using System;
using Amazon.RDS;
using Amazon.RDS.Model;
using System.IO;
using Amazon.Runtime;
using Amazon;
using System.Collections.Generic;


namespace RDS_Log_Downloader
{
    class Aws_Util
    {
        private AmazonRDSClient rds_client;

        public string db_instance_identifier { get; set; }
        public string log_base_path { get; set; }
        public List<Rdslogattr> rdslogattr = new List<Rdslogattr>();

        public Aws_Util(AWSCredentials awsCredentials, RegionEndpoint region)
        {
            //クライアント起動
            rds_client = new AmazonRDSClient(awsCredentials, region);
        }
        /// <summary>
        /// RDSからログファイルとその属性一覧を取得します。
        /// </summary>
        public void setRdsLogAttr()
        {
            //ファイル名一覧取得
            DescribeDBLogFilesRequest request = new DescribeDBLogFilesRequest();
            DescribeDBLogFilesResponse ret;
            request.DBInstanceIdentifier = db_instance_identifier;

            ret = rds_client.DescribeDBLogFiles(request);
            foreach (DescribeDBLogFilesDetails log in ret.DescribeDBLogFiles)
            {

                //タイムスタンプ取得
                var timestamp = log.LastWritten / 1000;
                DateTime dt_timestamp = UnixTime.FromUnixTime(timestamp);//POSIXからDateTimeに変換

                //余計なファイルを除外
                if (log.LogFileName == "mysqlUpgrade")
                {
                    continue;
                }
                //ログファイルの中身取得
                rdslogattr.Add(new Rdslogattr { download_log_file_name = log.LogFileName, dt_timestamp = dt_timestamp });
            }
        }

        /// <summary>
        /// 指定されたログファイルをRDSからダウンロードし、指定されたタイムスタンプでファイルを作成します。
        /// </summary>
        /// <param name="download_log_file_name">RDSからダウンロードするファイル名</param>
        /// <param name="save_log_file_name">保存するログファイル名</param>
        /// <param name="dt_timestamp">ログファイルにセットするタイムスタンプ</param>
        public void get_log_data(string download_log_file_name, string save_log_file_name, DateTime dt_timestamp)
        {
            bool additional_data_pending = true;//続きのデータがあるかのフラグ
            string marker = "0:0";//取得するデータの区切り文字
            int number_of_lines = 0;//取得する先頭ブロック指定

            //ファイルに保存
            using (StreamWriter sw = new StreamWriter(save_log_file_name, true))
            {
                while (additional_data_pending)
                {
                    DownloadDBLogFilePortionRequest request = new DownloadDBLogFilePortionRequest();
                    DownloadDBLogFilePortionResponse ret;
                    request.DBInstanceIdentifier = db_instance_identifier;
                    request.LogFileName = download_log_file_name;
                    request.Marker = marker;
                    request.NumberOfLines = number_of_lines;

                    ret = rds_client.DownloadDBLogFilePortion(request);

                    //ファイル書き込み
                    if (ret.LogFileData != null)
                    {
                        sw.Write(ret.LogFileData);
                    }

                    additional_data_pending = ret.AdditionalDataPending;//続きのデータがあるかのフラグ
                    marker = ret.Marker;//続きのデータを区切るマーカーセット

                }
            }
            //タイムスタンプ更新
            File.SetCreationTime(save_log_file_name, dt_timestamp);
            File.SetLastWriteTime(save_log_file_name, dt_timestamp);
        }
    }
}
