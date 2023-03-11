using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.Services;
using TikAeroWebAPI.Classes;
using System.Collections.Specialized;

namespace TikAeroWebAPI
{
    /// <summary>
    /// Summary description for AgencyService
    /// </summary>
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class AgencyService : System.Web.Services.WebService
    {
        private string errMessage = string.Empty;
        [WebMethod]
        public string Register(AgencyRegisterRequest AgencyRegisterRequest)
        {
            if (AgencyRegistrationValidation(AgencyRegisterRequest) == true)
            {
                tikSystem.Web.Library.Agents objAgents = new tikSystem.Web.Library.Agents();
                tikSystem.Web.Library.ServiceClient srvClient = new tikSystem.Web.Library.ServiceClient();

                objAgents = srvClient.GetAgencySessionProfile(AgencyRegisterRequest.AgencyCode, 
                                                              string.Empty);

                if (objAgents != null && objAgents.Count > 0)
                {
                    if (objAgents[0].agency_code.ToUpper().Equals(AgencyRegisterRequest.AgencyCode.ToUpper()) &
                        objAgents[0].agency_logon.Equals(AgencyRegisterRequest.UserCode) &
                        objAgents[0].agency_password.Equals(AgencyRegisterRequest.Password))
                    {
                        //Clear agency read record.
                        objAgents.Clear();

                        //Start insert Agency information.
                        tikSystem.Web.Library.Agent objAgent = new tikSystem.Web.Library.Agent();
                        objAgent.agency_type_code = AgencyRegisterRequest.AgencyInput.application_type;
                        objAgent.agency_name = AgencyRegisterRequest.AgencyInput.agency_name;
                        objAgent.legal_name = AgencyRegisterRequest.AgencyInput.legal_name;
                        objAgent.iata_number = AgencyRegisterRequest.AgencyInput.iata_number;
                        objAgent.tax_id = AgencyRegisterRequest.AgencyInput.tax_id;
                        objAgent.email = AgencyRegisterRequest.AgencyInput.email;
                        objAgent.fax = AgencyRegisterRequest.AgencyInput.fax;
                        objAgent.phone = AgencyRegisterRequest.AgencyInput.phone;
                        objAgent.address_line1 = AgencyRegisterRequest.AgencyInput.address_line1;
                        objAgent.address_line2 = AgencyRegisterRequest.AgencyInput.address_line2;
                        objAgent.street = AgencyRegisterRequest.AgencyInput.street;
                        objAgent.state = AgencyRegisterRequest.AgencyInput.state;
                        objAgent.district = AgencyRegisterRequest.AgencyInput.district;
                        objAgent.province = AgencyRegisterRequest.AgencyInput.province;
                        objAgent.city = AgencyRegisterRequest.AgencyInput.city;
                        objAgent.zip_code = AgencyRegisterRequest.AgencyInput.zip_code;
                        objAgent.po_box = AgencyRegisterRequest.AgencyInput.po_box;
                        objAgent.website_address = AgencyRegisterRequest.AgencyInput.website_address;
                        objAgent.contact_person = AgencyRegisterRequest.AgencyInput.contact_person;
                        objAgent.lastname = AgencyRegisterRequest.AgencyInput.lastname;
                        objAgent.firstname = AgencyRegisterRequest.AgencyInput.firstname;
                        objAgent.title_rcd = AgencyRegisterRequest.AgencyInput.title_rcd;
                        objAgent.user_logon = AgencyRegisterRequest.AgencyInput.user_logon;
                        objAgent.agency_password = AgencyRegisterRequest.AgencyInput.agency_password;
                        objAgent.country_rcd = AgencyRegisterRequest.AgencyInput.country_rcd;
                        objAgent.currency_rcd = AgencyRegisterRequest.AgencyInput.currency_rcd;
                        objAgent.language_rcd = AgencyRegisterRequest.AgencyInput.language_rcd;
                        objAgent.comment = AgencyRegisterRequest.AgencyInput.comment;

                        objAgents.Add(objAgent);

                        if (objAgents.Register(AgencyRegisterRequest.AgencyInput.application_type) == true)
                        {
                            return Utils.ErrorXml("000", "Success Request Transaction.");
                        }
                        else
                        {
                            return Utils.ErrorXml("5028", "Register Failed.");
                        }

                    }
                    else
                    {
                        return Utils.ErrorXml("5029", "Authentication Failed");
                    }

                }
                else
                {
                    return Utils.ErrorXml("5030", "Agency not found.");
                }
                
            }
            else
            {
                return errMessage;
            }
            
        }

        #region Helper Class
        private bool AgencyRegistrationValidation(AgencyRegisterRequest AgencyRegisterRequest)
        {
            NameValueCollection Setting = (NameValueCollection)System.Configuration.ConfigurationManager.GetSection("AgencyRegisterFieldSetting");

            if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyCode))
            {
                errMessage = Utils.ErrorXml("5000", "Agency Code Required");
                return false;
            } 
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.UserCode))
            {
                errMessage = Utils.ErrorXml("5001", "User Code Required");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.Password))
            {
                errMessage = Utils.ErrorXml("5002", "Password Required");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyInput.application_type) & 
                    tikSystem.Web.Library.ConfigurationHelper.ToBoolean("application_type") == true)
            {
                errMessage = Utils.ErrorXml("5003", "Agency Type Required");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyInput.agency_name) & 
                    tikSystem.Web.Library.ConfigurationHelper.ToBoolean("agency_name") == true) 
            {
                errMessage = Utils.ErrorXml("5004", "Agency name Required");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyInput.legal_name) & 
                    tikSystem.Web.Library.ConfigurationHelper.ToBoolean("legal_name") == true)
            {
                errMessage = Utils.ErrorXml("5005", "Legal name Required");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyInput.iata_number) & 
                    tikSystem.Web.Library.ConfigurationHelper.ToBoolean("iata_number") == true)
            {
                errMessage = Utils.ErrorXml("5006", "IATA number Required");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyInput.tax_id) & 
                    tikSystem.Web.Library.ConfigurationHelper.ToBoolean("tax_id") == true)
            {
                errMessage = Utils.ErrorXml("5007", "Tax Id Required");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyInput.phone) & 
                    tikSystem.Web.Library.ConfigurationHelper.ToBoolean("phone") == true)
            {
                errMessage = Utils.ErrorXml("5008", "Phone Required");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyInput.fax) & 
                    tikSystem.Web.Library.ConfigurationHelper.ToBoolean("fax") == true)
            {
                errMessage = Utils.ErrorXml("5009", "Fax Required");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyInput.email) & 
                    tikSystem.Web.Library.ConfigurationHelper.ToBoolean("email") == true)
            {
                errMessage = Utils.ErrorXml("5010", "Email Required");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyInput.website_address) & 
                    tikSystem.Web.Library.ConfigurationHelper.ToBoolean("website_address") == true) 
            {
                errMessage = Utils.ErrorXml("5011", "Website Address Required");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyInput.currency_rcd) & 
                    tikSystem.Web.Library.ConfigurationHelper.ToBoolean("currency_rcd") == true)
            {
                errMessage = Utils.ErrorXml("5012", "Currency Code Required");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyInput.language_rcd) & 
                    tikSystem.Web.Library.ConfigurationHelper.ToBoolean("language_rcd") == true)
            {
                errMessage = Utils.ErrorXml("5013", "Language Code Required");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyInput.contact_person) & 
                    tikSystem.Web.Library.ConfigurationHelper.ToBoolean("contact_person") == true)
            {
                errMessage = Utils.ErrorXml("5014", "Contact Person Required");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyInput.address_line1) & 
                    tikSystem.Web.Library.ConfigurationHelper.ToBoolean("address_line1") == true)
            {
                errMessage = Utils.ErrorXml("5015", "Address line1 Required");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyInput.address_line2) & 
                    tikSystem.Web.Library.ConfigurationHelper.ToBoolean("address_line2") == true)
            {
                errMessage = Utils.ErrorXml("5016", "Address line2 Required");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyInput.street) & 
                    tikSystem.Web.Library.ConfigurationHelper.ToBoolean("street") == true)
            {
                errMessage = Utils.ErrorXml("5017", "Street Required");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyInput.state) & 
                    tikSystem.Web.Library.ConfigurationHelper.ToBoolean("state") == true)
            {
                errMessage = Utils.ErrorXml("5018", "State Required");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyInput.province) & 
                    tikSystem.Web.Library.ConfigurationHelper.ToBoolean("province") == true) 
            {
                errMessage = Utils.ErrorXml("5019", "Province Required");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyInput.city) & 
                    tikSystem.Web.Library.ConfigurationHelper.ToBoolean("city") == true)
            {
                errMessage = Utils.ErrorXml("5020", "City Required");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyInput.zip_code) & 
                    tikSystem.Web.Library.ConfigurationHelper.ToBoolean("zip_code") == true) 
            {
                errMessage = Utils.ErrorXml("5021", "Zip Code Required");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyInput.country_rcd) &
                    tikSystem.Web.Library.ConfigurationHelper.ToBoolean("country_rcd") == true)
            {
                errMessage = Utils.ErrorXml("5022", "Country code Required");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyInput.lastname) &
                tikSystem.Web.Library.ConfigurationHelper.ToBoolean("lastname") == true)
            {
                errMessage = Utils.ErrorXml("5023", "Lastname Required.");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyInput.firstname) &
            tikSystem.Web.Library.ConfigurationHelper.ToBoolean("firstname") == true)
            {
                errMessage = Utils.ErrorXml("5024", "Firstname Required.");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyInput.title_rcd) &
            tikSystem.Web.Library.ConfigurationHelper.ToBoolean("title_rcd") == true)
            {
                errMessage = Utils.ErrorXml("5025", "Title Required.");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyInput.agency_password) &
                    tikSystem.Web.Library.ConfigurationHelper.ToBoolean("agency_password") == true)
            {
                errMessage = Utils.ErrorXml("5026", "Agency Password Required.");
                return false;
            }
            else if (string.IsNullOrEmpty(AgencyRegisterRequest.AgencyInput.comment) &
                    tikSystem.Web.Library.ConfigurationHelper.ToBoolean("comment") == true)
            {
                errMessage = Utils.ErrorXml("5027", "Comment Required.");
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
    }
}
