declare @c uniqueidentifier
while(1=1)
begin
    select top 1 @c = conversation_handle from dbo.SOTestChangeMessages
    if (@@ROWCOUNT = 0)
	begin
		break
	end
    end conversation @c with cleanup
end