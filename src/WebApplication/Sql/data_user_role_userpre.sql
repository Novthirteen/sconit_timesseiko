--user
--role
--userpre


INSERT INTO "ACC_User" (USR_Code,USR_Pwd,USR_FirstName,USR_LastName,USR_Email,USR_Address,USR_Sex,USR_Phone,USR_MPhone,IsActive,LastModifyDate,LastModifyUser) VALUES ('su','5051F702B43DFD1050DE942324F81108','超级','管理员','scms@yfkey.com','','M','','',1,{ts '2010-07-19 11:39:25.'},'lyt');

INSERT INTO "ACC_User" (USR_Code,USR_Pwd,USR_FirstName,USR_LastName,USR_Email,USR_Address,USR_Sex,USR_Phone,USR_MPhone,IsActive,LastModifyDate,LastModifyUser) VALUES ('Monitor','5F4DCC3B5AA765D61D8327DEB882CF99','Sys','tem','monitor@yfkey.com','','M','','',1,{ts '2010-07-07 14:14:11.'},'su')



INSERT INTO "ACC_Role" (ROLE_Code,ROLE_Desc,ROLE_AllowDel) VALUES ('administrators','管理员',0)




INSERT INTO "ACC_UserPre" (USR_Code,PreCode,PreValue) VALUES ('su','Language','zh-CN')


INSERT INTO "ACC_UserPre" (USR_Code,PreCode,PreValue) VALUES ('su','ThemeFrame','Picture')


INSERT INTO "ACC_UserPre" (USR_Code,PreCode,PreValue) VALUES ('su','ThemePage','Default')
GO