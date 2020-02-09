create table [gj].Rating
(
Id int primary key identity not null,
UserId nvarchar(450) references dbo.AspNetUsers(Id) not null,
ProductId int references gj.Products(Id) not null,
Rating float null,
Comment nvarchar(max) null
unique (UserId, ProductId)
)
