using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenRenewer.Models;

namespace TokenRenewer.Helpers
{
    class SqlFunctions
    {
        public static TokenRequestInfo GetTokenRequestInfo()
        {
            using (SqlConnection conn = SqlConnector.GetConnection())
            {
                TokenRequestInfo tokenRequestInfo = null;
                if (conn.State == ConnectionState.Open)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "[SP_GetTokenRequestInfo]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows && reader.Read())
                    {
                        tokenRequestInfo = new TokenRequestInfo();
                        tokenRequestInfo.grant_type = (string)reader["grant_type"];
                        tokenRequestInfo.client_id = (string)reader["client_id"];
                        tokenRequestInfo.client_secret = (string)reader["client_secret"];
                        tokenRequestInfo.scope = (string)reader["scope"];
                        tokenRequestInfo.session_id = (string)reader["session_id"];

                        tokenRequestInfo.request_url_0 = (string)reader["request_url_0"];
                        tokenRequestInfo.request_url_1 = (string)reader["request_url_1"];
                        tokenRequestInfo.renew_interval = (int)reader["renew_interval"];
                    }
                }
                return tokenRequestInfo;
            }
        }

        public static int TokenInfoUpdate(TokenInfo token)
        {
            using (SqlConnection conn = SqlConnector.GetConnection())
            {
                if (conn.State == ConnectionState.Open)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "[SP_TokenInfoUpdate]";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("access_token", token.access_token);
                    cmd.Parameters.AddWithValue("expires_in", token.expires_in);
                    cmd.Parameters.AddWithValue("token_type", token.token_type);
                    cmd.Parameters.AddWithValue("scope", token.scope);
                    cmd.Parameters.AddWithValue("last_update_time", token.last_update_time);
                    cmd.Parameters.AddWithValue("last_update_status", token.last_update_status);

                    return cmd.ExecuteNonQuery();
                }
            }
            return 0;
        }

        public static TokenRenewerConfig GetRenewerConfig()
        {
            using (SqlConnection conn = SqlConnector.GetConnection())
            {
                TokenRenewerConfig config = null;
                if (conn.State == ConnectionState.Open)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "[SP_GetRenewerConfig]";
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader reader = cmd.ExecuteReader();
                    if(reader.HasRows && reader.Read())
                    {
                        config = new TokenRenewerConfig();
                        config.StartStatus = (string)reader["StartStatus"];
                        config.StopStatus = (string)reader["StopStatus"];
                        config.AutoRenewButtonContentWhenStart = (string)reader["AutoRenewButtonContentWhenStart"];
                        config.AutoRenewButtonContentWhenStop = (string)reader["AutoRenewButtonContentWhenStop"];
                        config.AutoStartRenewer = (bool)reader["AutoStartRenewer"];
                        config.AutoRestartRenewer = (bool)reader["AutoRestartRenewer"];
                        config.RestartRenewerAfter = (int)reader["RestartRenewerAfter"];
                        config.RenewInterval = (int)reader["RenewInterval"];
                        config.TimerInterval = (int)reader["TimerInterval"];
                        config.ScrollViewerHeader = (string)reader["ScrollViewerHeader"];
                        config.RenewSuccessMessage = (string)reader["RenewSuccessMessage"];
                        config.RenewFailedMessage = (string)reader["RenewFailedMessage"];
                    }
                }
                return config;
            }
        }

        public static int TokenRenewerConfigUpdate(TokenRenewerConfig config)
        {
            using (SqlConnection conn = SqlConnector.GetConnection())
            {
                if (conn.State == ConnectionState.Open)
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "[SP_TokenRenewerConfigUpdate]";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("AutoStartRenewer", config.AutoStartRenewer);
                    cmd.Parameters.AddWithValue("AutoRestartRenewer", config.AutoRestartRenewer);

                    return cmd.ExecuteNonQuery();
                }
            }
            return 0;
        }
    }
}
