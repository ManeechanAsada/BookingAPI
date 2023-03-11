using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;
using System.Data;

namespace tikAeroWebMain
{
    public class ComplusMessage : RunComplus
    {
        public ComplusMessage() : base() { }

        public bool QueueMail(string strFromAddress,
                             string strFromName,
                             string strToAddress,
                             string strToAddressCC,
                             string strToAddressBCC,
                             string strReplyToAddress,
                             string strSubject,
                             string strBody,
                             string strDocumentType,
                             string strAttachmentStream,
                             string strAttachmentFileName,
                             string strAttachmentFileType,
                             string strAttachmentParser,
                             bool bHtmlBody,
                             bool bConvertAttachmentFromHTML2PDF,
                             bool bRemoveFromQueue,
                             tikAeroProcess.MAIL_PRIORITY lMailPriority,
                             string strUserId,
                             string strBookingId,
                             string strVoucherId,
                             string strBookingSegmentID,
                             string strPassengerId,
                             string strClientProfileId,
                             string strDocumentId,
                             string strLanguageCode)
        {
            tikAeroProcess.Message objMessage = null;
            bool bResult = false;

            try
            {
                if (_server.Length > 0)
                {
                    Type remote = Type.GetTypeFromProgID("tikAeroProcess.Message", _server);
                    objMessage = (tikAeroProcess.Message)Activator.CreateInstance(remote);
                    remote = null;
                }
                else
                { objMessage = new tikAeroProcess.Message(); }


                bResult = objMessage.QueueMail(strFromAddress,
                                                strFromName,
                                                strToAddress,
                                                strToAddressCC,
                                                strToAddressBCC,
                                                strReplyToAddress,
                                                strSubject,
                                                strBody,
                                                ref strDocumentType,
                                                ref strAttachmentStream,
                                                ref strAttachmentFileName,
                                                ref strAttachmentFileType,
                                                ref strAttachmentParser,
                                                ref bHtmlBody,
                                                ref bConvertAttachmentFromHTML2PDF,
                                                ref bRemoveFromQueue,
                                                ref lMailPriority,
                                                ref strUserId,
                                                ref strBookingId,
                                                ref strVoucherId,
                                                ref strBookingSegmentID,
                                                ref strPassengerId,
                                                ref strClientProfileId,
                                                ref strDocumentId,
                                                ref strLanguageCode);
            }
            catch
            { }
            finally
            {
                if (objMessage != null)
                { Marshal.FinalReleaseComObject(objMessage); }
                objMessage = null;
            }

            return bResult;
        }
    }
}