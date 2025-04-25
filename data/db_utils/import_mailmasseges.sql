/****** step 1  ******/
DELETE FROM [rental_in_romeDB_old].[dbo].[MAIL_TBL_MESSAGE]
GO
SET IDENTITY_INSERT [rental_in_romeDB_old].[dbo].[MAIL_TBL_MESSAGE] ON
INSERT INTO [rental_in_romeDB_old].[dbo].[MAIL_TBL_MESSAGE]
           ([id]
           ,[pid_request]
           ,[pid_request_state]
           ,[UidOnServer]
           ,[MessageID]
           ,[pid_user]
           ,[from_email]
           ,[from_name]
           ,[to_email]
           ,[to_name]
           ,[subject]
           ,[body_html_text]
           ,[body_plain_text]
           ,[date_sent]
           ,[date_received]
           ,[date_imported]
           ,[is_new])
SELECT * FROM [rental_in_romeDB].[dbo].[MAIL_TBL_MESSAGE]
GO
SET IDENTITY_INSERT [rental_in_romeDB_old].[dbo].[MAIL_TBL_MESSAGE] OFF


	
/****** step 2  ******/

DELETE FROM [rental_in_romeDB].[dbo].[MAIL_TBL_MESSAGE]
GO
SET IDENTITY_INSERT [rental_in_romeDB].[dbo].[MAIL_TBL_MESSAGE] ON
INSERT INTO [rental_in_romeDB].[dbo].[MAIL_TBL_MESSAGE]
           ([id]
           ,[pid_request]
           ,[pid_request_state]
           ,[UidOnServer]
           ,[MessageID]
           ,[pid_user]
           ,[from_email]
           ,[from_name]
           ,[to_email]
           ,[to_name]
           ,[subject]
           ,[body_html_text]
           ,[body_plain_text]
           ,[date_sent]
           ,[date_received]
           ,[date_imported]
           ,[is_new])
SELECT * FROM [rental_in_romeDB_old].[dbo].[MAIL_TBL_MESSAGE]
GO
SET IDENTITY_INSERT [rental_in_romeDB].[dbo].[MAIL_TBL_MESSAGE] OFF


