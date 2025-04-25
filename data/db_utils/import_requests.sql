/****** step 1  ******/
DELETE FROM [rental_in_romeDB_old].[dbo].[RNT_TBL_REQUEST]
GO
SET IDENTITY_INSERT [rental_in_romeDB_old].[dbo].[RNT_TBL_REQUEST] ON
INSERT INTO [rental_in_romeDB_old].[dbo].[RNT_TBL_REQUEST]
		([id]
      ,[pid_city]
      ,[pid_reservation]
      ,[pid_related_request]
      ,[pid_creator]
      ,[pid_operator]
      ,[operator_date]
      ,[pid_lang]
      ,[email]
      ,[name_full]
      ,[name_honorific]
      ,[name_first]
      ,[name_last]
      ,[phone]
      ,[request_country]
      ,[request_choice_1]
      ,[request_choice_2]
      ,[request_transport]
      ,[request_area]
      ,[request_choices]
      ,[request_price_range]
      ,[request_services]
      ,[request_adult_num]
      ,[request_child_num]
      ,[request_child_num_min]
      ,[request_date_start]
      ,[request_date_end]
      ,[request_date_is_flexible]
      ,[request_notes]
      ,[request_ip]
      ,[request_date_created]
      ,[state_pid]
      ,[state_date]
      ,[state_pid_user]
      ,[state_subject]
      ,[state_body]
      ,[inner_notes]
      ,[mail_body]
      ,[is_deleted])
SELECT * FROM [rental_in_romeDB].[dbo].[RNT_TBL_REQUEST]
GO
SET IDENTITY_INSERT [rental_in_romeDB_old].[dbo].[RNT_TBL_REQUEST] OFF

DELETE FROM [rental_in_romeDB_old].[dbo].[RNT_RL_REQUEST_STATE]
GO
SET IDENTITY_INSERT [rental_in_romeDB_old].[dbo].[RNT_RL_REQUEST_STATE] ON
INSERT INTO [rental_in_romeDB_old].[dbo].[RNT_RL_REQUEST_STATE]
           ([id]
      ,[pid_request]
      ,[pid_state]
      ,[date_state]
      ,[subject]
      ,[body]
      ,[pid_user])
SELECT * FROM [rental_in_romeDB].[dbo].[RNT_RL_REQUEST_STATE]
GO
SET IDENTITY_INSERT [rental_in_romeDB_old].[dbo].[RNT_RL_REQUEST_STATE] OFF

DELETE FROM [rental_in_romeDB_old].[dbo].[RNT_RL_REQUEST_ITEM]
GO
SET IDENTITY_INSERT [rental_in_romeDB_old].[dbo].[RNT_RL_REQUEST_ITEM] ON
INSERT INTO [rental_in_romeDB_old].[dbo].[RNT_RL_REQUEST_ITEM]
           ([id]
      ,[pid_request]
      ,[pid_estate]
      ,[pid_zone]
      ,[notes]
      ,[sequence])
SELECT * FROM [rental_in_romeDB].[dbo].[RNT_RL_REQUEST_ITEM]
GO
SET IDENTITY_INSERT [rental_in_romeDB_old].[dbo].[RNT_RL_REQUEST_ITEM] OFF




/****** step 2  ******/

DELETE FROM [rental_in_romeDB].[dbo].[RNT_TBL_REQUEST]
where id<30000
GO
SET IDENTITY_INSERT [rental_in_romeDB].[dbo].[RNT_TBL_REQUEST] ON
INSERT INTO [rental_in_romeDB].[dbo].[RNT_TBL_REQUEST]
		([id]
      ,[pid_city]
      ,[pid_reservation]
      ,[pid_related_request]
      ,[pid_creator]
      ,[pid_operator]
      ,[operator_date]
      ,[pid_lang]
      ,[email]
      ,[name_full]
      ,[name_honorific]
      ,[name_first]
      ,[name_last]
      ,[phone]
      ,[request_country]
      ,[request_choice_1]
      ,[request_choice_2]
      ,[request_transport]
      ,[request_area]
      ,[request_choices]
      ,[request_price_range]
      ,[request_services]
      ,[request_adult_num]
      ,[request_child_num]
      ,[request_child_num_min]
      ,[request_date_start]
      ,[request_date_end]
      ,[request_date_is_flexible]
      ,[request_notes]
      ,[request_ip]
      ,[request_date_created]
      ,[state_pid]
      ,[state_date]
      ,[state_pid_user]
      ,[state_subject]
      ,[state_body]
      ,[inner_notes]
      ,[mail_body]
      ,[is_deleted])
SELECT * FROM [rental_in_romeDB_old].[dbo].[RNT_TBL_REQUEST]
GO
SET IDENTITY_INSERT [rental_in_romeDB].[dbo].[RNT_TBL_REQUEST] OFF

DELETE FROM [rental_in_romeDB].[dbo].[RNT_RL_REQUEST_STATE]
where pid_request<30000
GO
INSERT INTO [rental_in_romeDB].[dbo].[RNT_RL_REQUEST_STATE]
           ([pid_request]
      ,[pid_state]
      ,[date_state]
      ,[subject]
      ,[body]
      ,[pid_user])
SELECT [pid_request]
      ,[pid_state]
      ,[date_state]
      ,[subject]
      ,[body]
      ,[pid_user] FROM [rental_in_romeDB_old].[dbo].[RNT_RL_REQUEST_STATE]
GO

DELETE FROM [rental_in_romeDB].[dbo].[RNT_RL_REQUEST_ITEM]
where pid_request<30000
GO
INSERT INTO [rental_in_romeDB].[dbo].[RNT_RL_REQUEST_ITEM]
           ([pid_request]
      ,[pid_estate]
      ,[pid_zone]
      ,[notes]
      ,[sequence])
SELECT [pid_request]
      ,[pid_estate]
      ,[pid_zone]
      ,[notes]
      ,[sequence] FROM [rental_in_romeDB_old].[dbo].[RNT_RL_REQUEST_ITEM]
GO
