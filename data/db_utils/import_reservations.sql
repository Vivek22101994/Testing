DELETE FROM  [rental_in_romeDB_wart].[dbo].[_rirDB_tbAllocazione]
GO
SET IDENTITY_INSERT [rental_in_romeDB_wart].[dbo].[_rirDB_tbAllocazione] ON
INSERT INTO [rental_in_romeDB_wart].[dbo].[_rirDB_tbAllocazione]
           ([idAllocazione]
           ,[idRisorsa]
           ,[idCliente]
           ,[stato]
           ,[dataInizio]
           ,[dataFine]
           ,[persone]
           ,[prezzoFinale]
           ,[costoTotale]
           ,[codice]
           ,[dataScadenza]
           ,[notaScadenza]
           ,[dataScadenzaDue]
           ,[notaScadenzaDue]
           ,[taxi]
           ,[notificaTaxi]
           ,[pulizie]
           ,[notificaPulizie]
           ,[responsabile]
           ,[oraArrivo]
           ,[voloArrivo]
           ,[aereoporto]
           ,[noteAccoglienza]
           ,[dataServizio]
           ,[notaServizio]
           ,[utenteCreazione]
           ,[dataCreazione]
           ,[utenteModifica]
           ,[lastUpdate]
           ,[extra])
SELECT [idAllocazione]
      ,[idRisorsa]
      ,[idCliente]
      ,[stato]
      ,[dataInizio]
      ,[dataFine]
      ,[persone]
      ,[prezzoFinale]
      ,[costoTotale]
      ,[codice]
      ,[dataScadenza]
      ,[notaScadenza]
      ,[dataScadenzaDue]
      ,[notaScadenzaDue]
      ,[taxi]
      ,[notificaTaxi]
      ,[pulizie]
      ,[notificaPulizie]
      ,[responsabile]
      ,[oraArrivo]
      ,[voloArrivo]
      ,[aereoporto]
      ,[noteAccoglienza]
      ,[dataServizio]
      ,[notaServizio]
      ,[utenteCreazione]
      ,[dataCreazione]
      ,[utenteModifica]
      ,[lastUpdate]
      ,[extra]
  FROM [APPOWART].[dbo].[_rirDB_tbAllocazione]
  WHERE dataInizio>'2011-01-01'
GO
SET IDENTITY_INSERT [rental_in_romeDB_wart].[dbo].[_rirDB_tbAllocazione] OFF


/****** step 2  ******/
DELETE FROM  [rental_in_romeDB_wart].[dbo].[_rirDB_tbCliente]
GO
SET IDENTITY_INSERT [rental_in_romeDB_wart].[dbo].[_rirDB_tbCliente] ON
INSERT INTO [rental_in_romeDB_wart].[dbo].[_rirDB_tbCliente]
           ([idCliente]
           ,[titolo]
           ,[nome]
           ,[cognome]
           ,[indirizzo]
           ,[contatti]
           ,[nazionalita]
           ,[email]
           ,[note])
SELECT [idCliente]
      ,[titolo]
      ,[nome]
      ,[cognome]
      ,[indirizzo]
      ,[contatti]
      ,[nazionalita]
      ,[email]
      ,[note]
  FROM [APPOWART].[dbo].[_rirDB_tbCliente]
GO
SET IDENTITY_INSERT [rental_in_romeDB_wart].[dbo].[_rirDB_tbCliente] OFF


/****** step 3  ******/
SET IDENTITY_INSERT [rental_in_romeDB].[dbo].[RNT_TBL_RESERVATION] ON
INSERT INTO [rental_in_romeDB].[dbo].[RNT_TBL_RESERVATION]
			([id],[unique_id],[code],[password]
			,[pid_related_request],[pid_creator] ,[pid_operator],[pid_estate]
			,[cl_id],[cl_email] ,[cl_name_honorific] ,[cl_name_full]
			,[num_adult],[num_child_over],[num_child_min]
			,[dtStart], [dtStartTime],[dtEnd], [dtEndTime]
			,[state_pid] ,[state_date],[state_pid_user],[state_subject] ,[state_body]
			,[dtCreation] ,[block_expire],[block_pid_user] ,[block_comments],block_expire_hours
			,[pr_total]
			,[pr_part_commission_tf]
			,[pr_part_commission_total]
			,[pr_part_agency_fee]
			,[pr_part_payment_total]
			,[pr_part_owner]
			,[is_deleted]
			,[inner_notes]
			,[is_booking]
			,[is_dtStartTimeChanged]
			,[is_dtEndTimeChanged]
			,[cl_isCompleted]
			,[limo_isCompleted]
			,[limo_in_isRequested]
			,[limo_out_isRequested]
			)
SELECT [idAllocazione],NEWID(),[codice],''
		,0,1 ,0,[idRisorsa]
		,res.[idCliente],[email],[titolo],[cognome]+' '+[nome]
		,[persone] ,0 ,0
		,[dataInizio], '140000' ,[dataFine], '110000'
		,lk.id ,[lastUpdate],0,[stato],''
		,[dataCreazione] ,[dataScadenza],1,[notaScadenza],48
		,[prezzoFinale]
		,[costoTotale]
		,[costoTotale]
		,0
		,[costoTotale]
		,[prezzoFinale]-[costoTotale]
		,0
		,N'\n [taxi]:' + [taxi]
		+'\n [notificaTaxi]:' + CONVERT(nvarchar, [notificaTaxi])
		+'\n [pulizie]:' + [pulizie]
		+'\n [notificaPulizie]:' + CONVERT(nvarchar, [notificaPulizie])
		+'\n [responsabile]:' + [responsabile]
		+'\n [oraArrivo]:' + [oraArrivo]
		+'\n [voloArrivo]:' + [voloArrivo]
		+'\n [aereoporto]:' + [aereoporto]
		+'\n [noteAccoglienza]:' + [noteAccoglienza]
		+'\n [dataServizio]:' + CONVERT(nvarchar, [dataServizio])
		+'\n [notaServizio]:' + [notaServizio]
		+'\n [utenteCreazione]:' + [utenteCreazione]
		+'\n [utenteModifica]:' + [utenteModifica]
		+'\n [lastUpdate]:' +CONVERT(nvarchar, [lastUpdate])
		+'\n [extra]:' + [extra]
		,case when res.[idCliente]>-1 then 1 else 0 end
		,0
		,0
		,0
		,0
		,0
		,0
		FROM [rental_in_romeDB_wart].[dbo].[_rirDB_tbAllocazione] res
		inner join [rental_in_romeDB].[dbo].[RNT_LK_RESERVATION_STATE] lk on res.[stato]=lk.abbr
		left join [rental_in_romeDB_wart].[dbo].[_rirDB_tbCliente] cl on res.[idCliente]=cl.[idCliente]
		WHERE res.dataInizio>'2011-01-01' and res.idAllocazione not in (SELECT id from [rental_in_romeDB].[dbo].[RNT_TBL_RESERVATION])
GO
SET IDENTITY_INSERT [rental_in_romeDB].[dbo].[RNT_TBL_RESERVATION] OFF
