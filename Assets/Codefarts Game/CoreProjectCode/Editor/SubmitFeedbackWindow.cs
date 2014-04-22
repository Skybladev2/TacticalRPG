/*
<copyright>
  Copyright (c) 2012 Codefarts
  All rights reserved.
  contact@codefarts.com
  http://www.codefarts.com
</copyright>
*/

namespace Codefarts.CoreProjectCode
{
    using System;
    using System.Collections.Specialized;
    using System.Net;
    using System.Xml;

    using Codefarts.Localization;

    using UnityEditor;

    using UnityEngine;

    /// <summary>
    /// Provides a window for allowing the user to send feedback to the author.
    /// </summary>
    public class SubmitFeedbackWindow : EditorWindow
    {
        /// <summary>
        /// Holds a reference to a web client object responsible for sending the feedback.
        /// </summary>
        private readonly WebClient client;

        /// <summary>
        /// Holds the value given for the email.
        /// </summary>
        private string email = string.Empty;

        /// <summary>
        /// Holds the string containing an error message.
        /// </summary>
        private string errorMessage = string.Empty;

        /// <summary>
        /// Holds the submitting state.
        /// </summary>
        private bool isSubmitting;

        /// <summary>
        /// Holds the value of the message.
        /// </summary>
        private string message = string.Empty;

        /// <summary>
        /// Holds the scroll position of the message text box.
        /// </summary>
        private Vector2 messageScroll;

        /// <summary>
        /// Holds a message containing the status of the submission.
        /// </summary>
        private string statusMessage = string.Empty;

        /// <summary>
        /// Holds the value for subject.
        /// </summary>
        private string subject = string.Empty;

        /// <summary>
        /// Holds a value indicating whether or not the feedback was successfully submitted.
        /// </summary>
        private bool successfullySubmitted;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitFeedbackWindow"/> class.
        /// </summary>
        public SubmitFeedbackWindow()
        {
            // create and hook into the web client
            this.client = new WebClient();
            this.client.UploadValuesCompleted += this.UploadValuesCompleted;
            this.client.UploadProgressChanged += this.UploadProgressChanged;
        }

        /// <summary>
        /// Provides a menu item entry t show this window.
        /// </summary>
        [MenuItem("Window/Codefarts/Error Reporting and Feedback")]
        public static void ShowFeedbackWindow()
        {
            // get the window, show it, and hand it focus
            var local = LocalizationManager.Instance;
            GetWindow<SubmitFeedbackWindow>(local.Get("Feedback"), true);
        }

        /// <summary>
        /// Will be called by unity when the windows needs to be redrawn.
        /// </summary>
        public void OnGUI()
        {
            // get reference to the localization manager.
            var local = LocalizationManager.Instance;

            GUILayout.BeginVertical();

            // check if we are in a submitting state
            if (!this.isSubmitting)
            {
                // draw email field
                GUILayout.Label(local.Get("EmailOptional"));
                this.email = GUILayout.TextField(this.email);

                // draw subject field
                GUILayout.Label(local.Get("Subject"));
                this.subject = GUILayout.TextField(this.subject);

                // draw message area
                GUILayout.Label(local.Get("Message"));
                this.messageScroll = GUILayout.BeginScrollView(this.messageScroll, false, false);
                this.message = GUILayout.TextArea(this.message);
                GUILayout.EndScrollView();

                // if there is a error message then show it
                if (this.errorMessage != null)
                {
                    GUILayout.Label(this.errorMessage, "ErrorLabel");
                }

                // draw the submit button
                GUILayout.BeginHorizontal();
                if (GUILayout.Button(local.Get("Submit")))
                {
                    // set submitting state and set initial status message
                    this.isSubmitting = true;
                    this.statusMessage = local.Get("Submitting");

                    // setup the data that will be submitted
                    var data = new NameValueCollection
                                   {
                                       { "email", this.email },
                                       { "subject", this.subject },
                                       { "message", this.message }
                                   };

                    // attempt to parse the url and transmit the message data
                    Uri url;
                    if (Uri.TryCreate("http://www.codefarts.com/submitfeedback", UriKind.Absolute, out url))
                    {
                        // asynchronously post the message
                        this.client.UploadValuesAsync(url, "POST", data);
                    }
                    else
                    {
                        // report error if could not parse the destination url
                        this.errorMessage = local.Get("ERR_CouldNotParseUri");
                    }
                }

                // add some flexible space to separate the buttons and draw the close button.
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(local.Get("Close")))
                {
                    // be sure to cancel submission is closing
                    this.client.CancelAsync();
                    this.Close();
                }

                GUILayout.EndHorizontal();
            }
            else
            {
                // report submission status
                GUILayout.Label(this.statusMessage);

                // constantly repaint while submitting message
                this.Repaint();
            }

            GUILayout.EndVertical();
        }

        /// <summary>
        /// Called 100 times per second by the unity editor.
        /// </summary>
        public void Update()
        {
            // get reference to the localization manager.
            var local = LocalizationManager.Instance;

            // check if submission was a success
            if (this.successfullySubmitted)
            {
                // display confirmation dialog
                EditorUtility.DisplayDialog(local.Get("Success"), this.statusMessage, local.Get("Close"));
                this.successfullySubmitted = false;
                this.Close();
            }
        }

        /// <summary>
        /// Used to handle the web client upload progress event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An System.EventArgs that contains no event data.</param>
        private void UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            var local = LocalizationManager.Instance;
            this.statusMessage = string.Format(local.Get("PercentageComplete"), e.ProgressPercentage);
        }

        /// <summary>
        /// Used to handle the web client upload values completed event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">An System.EventArgs that contains no event data.</param>
        private void UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            // is no longer submitting
            this.isSubmitting = false;

            // check if error occurred and if so report to console and set error message.
            if (e.Error != null)
            {
                Debug.LogException(e.Error);
                this.errorMessage = e.Error.Message;
                return;
            }

            // check if canceled submission
            var local = LocalizationManager.Instance;
            if (e.Cancelled)
            {
                this.errorMessage = local.Get("Canceled");
                return;
            }

            try
            {
                var data = System.Text.Encoding.UTF8.GetString(e.Result);
                var json = Editor.JSON.Parse(data);
                  // attempt to get the success and message element values
                var messageElement = json["Message"].Value;
                var successElement = json["Success"].AsBool;
               
                // if not successful report the error
                if (!successElement)
                {
                    this.errorMessage = messageElement;
                    return;
                }

                // set status to the message.
                this.statusMessage = messageElement;
                this.successfullySubmitted = true;
               
                //if (this.HandleXmlResponse(e, local))
                //{
                //    return;
                //}
            }
            catch (Exception ex)
            {
                // if something goes wrong report it
                this.errorMessage = ex.Message;
                return;
            }

            // cleanup variables
            this.errorMessage = null;
            this.statusMessage = string.Empty;
            this.email = string.Empty;
            this.subject = string.Empty;
            this.message = string.Empty;
        }

        private bool HandleXmlResponse(UploadValuesCompletedEventArgs e, LocalizationManager local)
        {
            // read the result data as xml
            var doc = new XmlDocument();
            var xml = System.Text.Encoding.UTF8.GetString(e.Result);
            doc.LoadXml(xml);

            // validate the the xml root node is 'result'
            if (doc.DocumentElement != null && doc.DocumentElement.Name == "result")
            {
                // attempt to get the success and message element values
                var messageElement = doc.DocumentElement["message"];
                var successElement = doc.DocumentElement["success"];

                if (messageElement == null)
                {
                    throw new XmlException(local.Get("ERR_CouldNotFindMessageElement"));
                }

                if (successElement == null)
                {
                    throw new XmlException(local.Get("ERR_CouldNotFindSuccessElement"));
                }

                bool successValue;
                if (!bool.TryParse(successElement.InnerText, out successValue))
                {
                    throw new XmlException(local.Get("ERR_CouldNotParseSuccessElement"));
                }

                var messageValue = messageElement.InnerText;

                // if not successful report the error
                if (!successValue)
                {
                    this.errorMessage = messageValue;
                    return true;
                }

                // set status to the message.
                this.statusMessage = messageValue;
                this.successfullySubmitted = true;
            }

            return false;
        }
    }
}