CREATE UNIQUE NONCLUSTERED INDEX [UX_UnconsumedMessage] 
  ON [dbo].[UnconsumedMessage] ( [clientname] ASC, [messageid] ASC ) 
  WITH (pad_index = OFF, statistics_norecompute = OFF, sort_in_tempdb = OFF, 
ignore_dup_key = OFF, drop_existing = OFF, online = OFF, allow_row_locks = ON, 
allow_page_locks = ON) ON [PRIMARY]
