CREATE TABLE [dbo].[unconsumedmessage] 
  ( 
     [unconsumedmessageid] [BIGINT] IDENTITY(1, 1) NOT NULL, 
     [clientname]          [NVARCHAR](100) NOT NULL, 
     [messageid]           [UNIQUEIDENTIFIER] NOT NULL, 
     CONSTRAINT [PK_UnconsumedMessage] PRIMARY KEY CLUSTERED ( 
     [unconsumedmessageid] ASC )WITH (pad_index = OFF, statistics_norecompute = 
     OFF, ignore_dup_key = OFF, allow_row_locks = on, allow_page_locks = on) ON 
     [PRIMARY] 
  ) 
ON [PRIMARY] 
