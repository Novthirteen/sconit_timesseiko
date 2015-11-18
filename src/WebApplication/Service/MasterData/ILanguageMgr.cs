using com.Sconit.Service.Ext.MasterData;

using com.Sconit.Entity.MasterData;
namespace com.Sconit.Service.MasterData
{
    public interface ILanguageMgr
    {
        string ProcessLanguage(string content, string language);

        void ReLoadLanguage();

        string TranslateMessage(string content, string userCode);

        string TranslateMessage(string content, User user);

        string TranslateMessage(string content, string userCode, params string[] parameters);

        string TranslateMessage(string content, User user, params string[] parameters);

        string TranslateContent(string content, string language, params string[] parameters);
    }
}



#region Extend Interface


namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface ILanguageMgrE : com.Sconit.Service.MasterData.ILanguageMgr
    {
        
    }
}

#endregion

#region Extend Interface


namespace com.Sconit.Service.Ext.MasterData
{
    public partial interface ILanguageMgrE : com.Sconit.Service.MasterData.ILanguageMgr
    {
        
    }
}

#endregion
