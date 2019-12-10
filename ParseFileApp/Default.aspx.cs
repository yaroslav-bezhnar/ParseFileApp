using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ParseFileApp
{
    public partial class Default : Page
    {
        #region Constants

        private static readonly char[] END_OF_SENTENCE_CHARS = { '.', '!', '?' };

        #endregion

        #region Fields

        private string _currentWord;
        private SentenceManager _manager;
        private List<string> _sentences;

        #endregion

        #region Protected Methods

        protected void Page_Load( object sender, EventArgs e )
        {
            _manager = SentenceManager.GetInstance();

            fillSavedSentences();
        }

        protected void btnStart_Click( object sender, EventArgs e )
        {
            Logger.WriteLog( "Button 'Start' clicked." );

            if ( fileUpload.HasFile )
            {
                var wordToFind = txtEnteredText.Text.Trim();
                if ( !string.IsNullOrWhiteSpace( wordToFind ) )
                {
                    _currentWord = wordToFind;
                    saveData();
                }
                else
                {
                    Logger.WriteLog( "Word not entered.", MessageType.Warning );
                    showMessage( "Please enter the word." );
                }
            }
            else
            {
                Logger.WriteLog( "File not selected.", MessageType.Warning );
                showMessage( "Please select the file." );
            }
        }

        #endregion

        #region Private Methods

        private void readFile()
        {
            Logger.WriteLog( $"Reading file '{fileUpload.FileName}'." );

            try
            {
                var text = getStringFromBytes( fileUpload.FileBytes );
                if ( !string.IsNullOrWhiteSpace( text ) )
                {
                    Logger.WriteLog( "Splitting text to sentences." );

                    _sentences = text.Split( END_OF_SENTENCE_CHARS, StringSplitOptions.RemoveEmptyEntries ).ToList();
                }
            }
            catch ( HttpException ex )
            {
                Logger.WriteLog( $"HTTP Error. Message: {ex.Message}", MessageType.Error );
                clearInfo();
            }
            catch ( IOException ex )
            {
                Logger.WriteLog( $"Input/Output Error. Message: {ex.Message}", MessageType.Error );
                clearInfo();
            }
            catch ( Exception ex )
            {
                Logger.WriteLog( $"Error. Message: {ex.Message}", MessageType.Error );
                clearInfo();
            }
        }

        private void saveData()
        {
            readFile();
            findSentences();
            fillSavedSentences();
        }

        private void findSentences()
        {
            if ( !string.IsNullOrWhiteSpace( _currentWord ) && _sentences != null && _sentences.Any() )
            {
                foreach ( var sentence in _sentences )
                {
                    var matches = Regex.Matches( $@"\b{sentence}\b", _currentWord,
                                                RegexOptions.IgnoreCase | RegexOptions.Compiled );
                    if ( matches.Count > 0 )
                    {
                        _manager.AddInfoToDatabase( sentence, matches.Count );
                    }
                }
            }
        }

        private void clearInfo()
        {
            _currentWord = null;
            _sentences = null;
        }

        private static string getStringFromBytes( byte[] bytes )
        {
            try
            {
                using ( var stream = new MemoryStream( bytes ) )
                {
                    using ( var reader = new StreamReader( stream ) )
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        private void showMessage( string message )
        {
            Response.Write( $"<script>alert('{message}');</script>" );
        }

        private void fillSavedSentences()
        {
            Logger.WriteLog( "Reading sentences from database." );

            listBox.Items.Clear();

            var sentences = _manager.GetSavedSentences();
            if ( sentences.Any() )
            {
                sentences.Reverse();
                listBox.Items.AddRange( sentences.Select( el => new ListItem( el ) ).ToArray() );
            }
        }

        #endregion
    }
}