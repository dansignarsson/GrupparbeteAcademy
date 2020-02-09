
create table [gj].Products
(
	Id int primary key identity not null,
	[Name] nvarchar(100) not null,
	[Description] nvarchar(max) null,
	Price float null,
	Brand nvarchar(100) null,
	ImgURL nvarchar(max) null
	unique ([Name], Brand)
	
)

