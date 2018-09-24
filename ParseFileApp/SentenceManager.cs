#region
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
#endregion

namespace ParseFileApp
{
    /// <summary>
    ///   Provides a class to work with database.
    /// </summary>
    public sealed class SentenceManager
    {
        #region Constants
        public const string DB_CONNECTION =
            @"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = MyDataBase; Integrated Security = True";

        private static SentenceManager _instance;
        #endregion

        #region Fields
        private readonly SqlConnection _connection;
        #endregion

        #region Constructors
        /// <summary>
        ///   Initialize a new instance of the <see cref="SentenceManager"/> class.
        /// </summary>
        private SentenceManager()
        {
            _connection = new SqlConnection( DB_CONNECTION );
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///   Gets instance of the <see cref="SentenceManager"/> class.
        /// </summary>
        /// <returns></returns>
        public static SentenceManager GetInstance()
        {
            return _instance ?? ( _instance = new SentenceManager() );
        }

        /// <summary>
        ///   Adds information to table of database.
        /// </summary>
        /// <param name="sentence">Sentence to add.</param>
        /// <param name="count">Count of occurrences.</param>
        public void AddInfoToDatabase( string sentence, int count )
        {
            Logger.WriteLog( "Adding info to database." );

            try
            {
                openConnection();

                using ( var command =
                    new SqlCommand( $@"INSERT INTO Sentences (Sentence, Occurrences) VALUES ('{reverseString( sentence )}', {count})",
                                    _connection ) )
                {
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                closeConnection();
            }
        }

        /// <summary>
        ///   Gets saved sentences from table of database.
        /// </summary>
        /// <returns></returns>
        public List<string> GetSavedSentences()
        {
            Logger.WriteLog( "Getting sentences from database." );

            var result = new List<string>();

            try
            {
                openConnection();

                using ( var command = new SqlCommand( "SELECT Sentence From Sentences", _connection ) )
                {
                    using ( var reader = command.ExecuteReader() )
                    {
                        while ( reader.Read() )
                        {
                            result.Add( reverseString( reader.GetString( 0 ) ) );
                        }
                    }
                }
            }
            finally
            {
                closeConnection();
            }

            return result;
        }
        #endregion

        #region Private Methods
        private void openConnection()
        {
            if ( _connection != null && _connection.State != ConnectionState.Open )
            {
                _connection.Open();
            }
        }

        private void closeConnection()
        {
            if ( _connection != null && _connection.State == ConnectionState.Open )
            {
                _connection?.Close();
            }
        }

        private static string reverseString( string input )
        {
            if ( string.IsNullOrWhiteSpace( input ) )
            {
                return string.Empty;
            }

            return new string( input.Reverse().ToArray() );
        }
        #endregion
    }
}
