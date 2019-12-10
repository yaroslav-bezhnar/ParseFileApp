using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace ParseFileApp
{
    [Flags]
    public enum MessageType
    {
        [Description( "Info" )] Info,

        [Description( "Warning" )] Warning,

        [Description( "Error" )] Error
    }

    /// <summary>
    ///     Represents a class for logging Errors/Warnings/Information.
    /// </summary>
    public static class Logger
    {
        #region Constants

        private static readonly string _logTable;
        private static readonly SqlConnection _connection;

        #endregion

        #region Constructors

        static Logger()
        {
            _logTable = "LogInfo";
            _connection = new SqlConnection( SentenceManager.DB_CONNECTION );
        }

        #endregion

        #region Public Methogs

        /// <summary>
        ///     Write logging information to file.
        /// </summary>
        /// <param name="message">Message to write.</param>
        /// <param name="messageType">Type of message.</param>
        public static void WriteLog( string message, MessageType messageType = MessageType.Info )
        {
            try
            {
                checkConnection();

                using ( var command = new SqlCommand( $"INSERT INTO {_logTable} (Time, Message, Type) VALUES ('{DateTime.Now}', '{message}', '{messageType}')", _connection ) )
                {
                    command.ExecuteNonQuery();
                }
            }
            catch
            {
                // ignored
            }
        }

        #endregion

        #region Private Methods

        private static void checkConnection()
        {
            if ( _connection != null && _connection.State != ConnectionState.Open )
            {
                _connection.Open();
            }
        }

        #endregion
    }
}