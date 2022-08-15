namespace CISM_PJ.Common
{
    public class Common_Msg
    {
        public string Save_msg
        {
            get { return "Saved Successfully!"; }
        }
        public string Updated_msg
        {
            get { return "Updated Successfully!"; }
        }
        public string Deleted_msg
        {
            get { return "Deleted Successfully!"; }
        }
        public string Duplicated_msg
        {
            get { return " is already existed!"; }
        }
        public string Save_Err_msg
        {
            get { return "Saving in Error!"; }
        }
        public string Updated_Err_msg
        {
            get { return "Updating in Error!"; }
        }
        public string Deleted_Err_msg
        {
            get { return "Deleting in Error!"; }
        }

        public string Error_Msg = "An error occured in the solution! , Please contact with system administrator";
        public int deletedCode = -1;
        public int successcode = 0;
        public int duplicatedcode = -1;
        public int errorcode = -1;

        public string Deleting_Error_msg
        {
            get { return "Sorry You cannot delete it!', 'Data is using in another form.!"; }
        }
    }
}