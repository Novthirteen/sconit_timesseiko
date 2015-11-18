
--begin tiansu 20121011 

alter view BillView
as 
SELECT     dbo.ActBill.OrderNo, dbo.ActBill.Item, AVG(ISNULL(dbo.ActBill.BillAmount, 0)) AS BillAmount, AVG(ISNULL(dbo.ActBill.BilledAmount, 0)) AS BilledAmount, 
                      AVG(ISNULL(dbo.ActBill.BillAmount, 0) - ISNULL(dbo.ActBill.BilledAmount, 0)) AS NoBilledAmount, MIN(dbo.BillMstr.EffDate) AS MinEffDate, MAX(dbo.BillMstr.EffDate) 
                      AS MaxEffDate
FROM         dbo.ActBill LEFT OUTER JOIN
                      dbo.BillDet ON dbo.BillDet.TransId = dbo.ActBill.Id LEFT OUTER JOIN
                      dbo.BillMstr ON dbo.BillDet.BillNo = dbo.BillMstr.BillNo AND dbo.BillMstr.Status <> 'Void' AND dbo.BillMstr.Status <> 'Cancel'
GROUP BY dbo.ActBill.OrderNo, dbo.ActBill.Item

GO
--end tiansu

 
--begin tiansu 20110517 遗留数据
update orderdet set unitpriceafterdiscount = unitprice*(1.0-cast( discount/(unitprice* orderqty) as   numeric(9,2))) where unitpriceafterdiscount<0;
go
 --end tiansu 20110517


 --begin tiansu 20110422 徐天熙
 --订单号：GUH110001,型号：FD100676,原单价：500，现单价：590元
update orderdet set unitprice=590,UnitPriceAfterDiscount=590 where orderno='GUH110001';
update PlanBill set listprice=590,unitprice=690.3,planamount=20709,actamount=20709 where orderno='GUH110001'; 
update actBill set listprice=590,unitprice=690.3,billamount=20709 where orderno='GUH110001'; 
--因为前期王总他们出差，没人受权，又急着处理,所以STW110069第一项的单价收过部分货，不能重新计价了, 正确价格应该是826.20的，实在不好意思
update orderdet set unitprice=826.20,UnitPriceAfterDiscount=826.20 where orderno='STW110069' and item ='21000136';
update PlanBill set listprice=826.20,unitprice=826.20,planamount=7435.8,actamount=7435.8 where orderno='STW110069'and item ='21000136';
update actBill set listprice=826.20,unitprice=826.20,billamount=7435.8 where orderno='STW110069' and item ='21000136';
--end tiansu 20110422



--begin tiansu 20110402 修改 销售单TSC111198 的价格 sarah.dai@times-seiko.com
update orderdet set amountto=50500 ,includetaxamountto=50500*1.17 where orderno='TSC111198' and item ='21000051';
update orderdet set amountto=40000,includetaxamountto=40000*1.17 where orderno='TSC111198' and item ='21000052';
GO

--STW110016
update orderdet set includetaxamountfrom = 15412,amountfrom=15412 where item='21100030' and orderno='STW110016';
update orderdet set includetaxamountfrom = 43221.60,amountfrom=43221.60 where item='21400002' and orderno='STW110016';
update orderdet set includetaxamountfrom = 12340,amountfrom= 12340 where item='21400004' and orderno='STW110016';
GO
--end tiansu 20110402


--begin tiansu 20110402
update orderdet set includetaxamountfrom = 386.10,amountfrom=386.10 where item='25100006' and orderno='STW110064';
update orderdet set includetaxamountfrom = 122.85,amountfrom=122.85 where item='21100071' and orderno='STW110064';
update planbill set actamount=386.10,planamount=386.10,unitprice=77.22 where item='25100006' and orderno='STW110064'; 
update planbill set actamount=122.85,planamount=122.85,unitprice=24.57 where item='21100071' and orderno='STW110064'; 
update actbill set billamount=386.10,unitprice=77.22 where item='25100006' and orderno='STW110064'; 
update actbill set billamount=122.85,unitprice=24.57 where item='21100071' and orderno='STW110064'; 
GO
--end tiansu 20110402


--begin tiansu 20110401  修改价格逻辑，数据迁移
update ordermstr set taxcode = 17;
update orderdet set taxcode = 17;
update planbill set taxcode=17;
update billmstr set taxcode=17;
update billdet set taxcode=17;
GO


update flowmstr set pricelist = pricelistfrom where pricelistfrom is not null;
update flowmstr set pricelist = pricelistto where pricelistto is not null;
update flowmstr set pricelist = pricelistfrom where pricelistfrom is not null;
update flowmstr set pricelist = pricelistto where pricelistto is not null;
update ordermstr set pricelist = pricelistfrom where pricelistfrom is not null;
update ordermstr set pricelist = pricelistto where pricelistto is not null;
update ordermstr set Discount = Discountfrom where Discountfrom is not null;
update ordermstr set Discount = Discountto where Discountto is not null;
update orderdet set Discount = Discountfrom where Discountfrom is not null;
update orderdet set Discount = Discountto where Discountto is not null;
GO

update ordermstr  set ordermstr.isincludetax = pricelistmstr.isincludetax from ordermstr,pricelistmstr  where pricelistmstr.code=ordermstr.pricelist;
GO

update orderdet set orderdet.IsProvEst=pricelistdet.IsProvEst,orderdet.unitprice=pricelistdet.unitprice from orderdet ,pricelistdet where  orderdet.pricelistdetfrom= pricelistdet.id or orderdet.pricelistdetto =pricelistdet.id;
GO

--三条包含头折扣的order
update orderdet set headdiscount=excludetotalprice where orderno='SVW011103';
update orderdet set headdiscount=11200 where orderno='TSC101037';
update orderdet set headdiscount=excludetotalprice*0.106522057286222 where orderno='TSC111144';
GO

update orderdet set UnitPriceAfterDiscount = UnitPrice - isnull(discount,0) - isnull(headdiscount,0) / orderqty;
GO

update planbill set listprice = orderdet.unitprice,planbill.isincludetax= isnull(orderdet.isincludetax,0) from orderdet,planbill where orderdet.orderno=planbill.orderno 
 and orderdet.item = planbill.item 
GO


update planbill set unitprice = includetaxprice  from planbill,orderdet where planbill.orderno=orderdet.orderno and planbill.item=orderdet.item
update planbill set actamount=actqty* unitprice,planamount=planqty* unitprice;
GO


update actbill set listprice = orderdet.unitprice from orderdet,actbill where orderdet.orderno=actbill.orderno 
 and orderdet.item = actbill.item 
GO

update actbill set actbill.unitprice = planbill.unitprice  from actbill,planbill where actbill.orderno=planbill.orderno 
 and actbill.item = planbill.item 
GO
update actbill set billamount = billqty*unitprice,billedamount=billedqty*unitprice;
GO


update billdet set billdet.listprice = actbill.listprice,billdet.unitprice=actbill.unitprice  from billdet,actbill where billdet.transid=actbill.id;
GO
update billdet set orderamount=billedqty*unitprice,discount=0;


update bm set bm.totalbilldetailamount = bd.amount1,bm.totalbillamount = bd.amount2 from billmstr bm,
(select billno,sum(billdet.listprice * billdet.billedqty) amount1,sum(billdet.orderamount) amount2 from billdet group by billno) bd
where bm.billno=bd.billno





--end tiansu 20110401


--begin tiansu 20110401 给天熙的对账报表
--order
select om.type,om.orderno,od.item,isnull(od.unitprice,0),isnull(od.discount,0),od.orderqty,od.taxcode,(case od.isincludetax when 1 then '包含' else '不包含' end),isnull(od.unitpriceafterdiscount,0),od.includetaxprice,od.includetaxtotalprice from ordermstr om,orderdet od where om.orderno=od.orderno
order by om.orderno,od.item,om.type

--账单
select bm.billno,bm.totalbillamount,isnull(bm.backwashamount,0) from billmstr bm order by bm.transtype,bm.billno

--账单明细
select bm.transtype,bm.billno,bd.unitprice,bd.billedqty,bd.orderamount from billmstr bm,billdet bd where bm.billno=bd.billno order by bm.transtype,bm.billno

--付款
select paymentno,amount,backwashamount from payment order by paymentno;

--账单付款明细
select bp.billno,b.totalbillamount,bp.paymentno,p.amount,bp.backwashamount 
from billmstr b,payment p,billpayment bp where b.billno=bp.billno and bp.paymentno=p.paymentno order by bp.billno,bp.paymentno;


--end tiansu 20110401


--begin tiansu 20110401 价格逻辑修改

alter table FlowDet
add  PriceList	varchar(50)	null;
GO

alter table FlowMstr
add  PriceList	varchar(50)	null;
GO

alter table ordermstr
add TaxCode	decimal(18, 8) null,
IsIncludeTax bit null,
PriceList	varchar(50)	null,
Discount	decimal(18, 8) null;
GO


alter table orderdet
add PriceList	varchar(50)	null,
Discount	decimal(18, 8)	null,
HeadDiscount	decimal(18, 8)	null,
UnitPrice	decimal(18, 8)	null,
UnitPriceAfterDiscount	decimal(18, 8)	null,
TaxCode		decimal(18, 8) null,
IsProvEst	bit	null;
GO

alter table planbill
add ListPrice	decimal(18, 8)	null;
GO


alter table actbill
add ListPrice	decimal(18, 8)	null;
GO

alter table billdet
add 
	HeadDiscount	decimal(18, 8)	null,
	ListPrice	decimal(18, 8)	null;
GO




alter view OrderDetTracerView as
SELECT     dbo.OrderDet.Id, dbo.OrderDet.OrderNo, dbo.OrderDet.Item, dbo.OrderDet.RefItemCode, dbo.OrderDet.Seq, dbo.OrderDet.Uom, dbo.OrderDet.UC, dbo.OrderDet.ReqQty, 
                      dbo.OrderDet.OrderQty, dbo.OrderDet.ShipQty, dbo.OrderDet.RecQty, dbo.OrderDet.RejQty, dbo.OrderDet.ScrapQty, dbo.OrderDet.OrderGrLotSize, 
                      dbo.OrderDet.BatchSize, dbo.OrderDet.LocFrom, dbo.OrderDet.LocTo, dbo.OrderDet.BillFrom, dbo.OrderDet.BillTo, dbo.OrderDet.PriceList, 
                      dbo.OrderDet.Discount, OrderDet.TaxCode,OrderDet.UnitPrice,OrderDet.UnitPriceAfterDiscount,OrderDet.IsProvEst,
                      dbo.OrderDet.Bom, dbo.OrderDet.HuLotSize, dbo.OrderDet.BillSettleTerm, dbo.OrderDet.Customer, dbo.OrderDet.HeadDiscount,
                      dbo.OrderDet.PackVol, dbo.OrderDet.PackType, dbo.OrderDet.NeedInspect, dbo.OrderDet.IdMark, dbo.OrderDet.BarCodeType, dbo.OrderDet.ItemVersion, 
                      dbo.OrderDet.OddShipOpt, dbo.OrderDet.CustomerItemCode, dbo.OrderDet.DateField1, dbo.OrderDet.DateField2, dbo.OrderDet.DateField3, dbo.OrderDet.DateField4, 
                      dbo.OrderDet.NumField1, dbo.OrderDet.NumField2, dbo.OrderDet.NumField3, dbo.OrderDet.NumField4, dbo.OrderDet.NumField5, dbo.OrderDet.NumField6, 
                      dbo.OrderDet.NumField7, dbo.OrderDet.NumField8, dbo.OrderDet.TextField1, dbo.OrderDet.TextField2, dbo.OrderDet.TextField3, dbo.OrderDet.TextField4, 
                      dbo.OrderDet.TextField5, dbo.OrderDet.TextField6, dbo.OrderDet.TextField7, dbo.OrderDet.TextField8, dbo.OrderDet.Brand, dbo.OrderDet.Manufacturer, 
                      dbo.OrderDet.IsIncludeTax, dbo.OrderTracer.Code AS OrderTracerCode, dbo.OrderTracer.Item AS OrderTracerItem
FROM         dbo.OrderDet INNER JOIN
                      dbo.OrderLocTrans ON dbo.OrderLocTrans.OrderDetId = dbo.OrderDet.Id INNER JOIN
                      dbo.OrderTracer ON dbo.OrderTracer.RefOrderLocTransId = dbo.OrderLocTrans.Id
GO


--orderdet计算列
--IncludeTaxPrice
--(case [IsIncludeTax] when (1) then isnull([unitpriceafterdiscount],isnull([unitprice],(0))) else isnull([unitpriceafterdiscount],isnull([unitprice],(0)))*((1)+isnull([taxcode],(17))/(100.0)) end)
--IncludeTaxTotalPrice
--((case [IsIncludeTax] when (1) then isnull([unitpriceafterdiscount],isnull([unitprice],(0))) else isnull([unitpriceafterdiscount],isnull([unitprice],(0)))*((1)+isnull([taxcode],(17))/(100.0)) end)*[orderqty])
--ExcludeTaxPrice
--(case [IsIncludeTax] when (1) then isnull([unitpriceafterdiscount],isnull([unitprice],(0)))/((1)+isnull([taxcode],(17))/(100.0)) else isnull([unitpriceafterdiscount],isnull([unitprice],(0))) end)
--ExcludeTotalPrice
--(case [IsIncludeTax] when (1) then isnull([unitpriceafterdiscount],isnull([unitprice],(0)))/((1)+isnull([taxcode],(17))/(100.0)) else isnull([unitpriceafterdiscount],isnull([unitprice],(0))) end*[orderqty])


--order中重新计价按钮权限
INSERT INTO "ACC_Permission" (PM_Code, PM_Desc, PM_CateCode) VALUES ('RecalculateOrder', '重新计价', 'OrderOperation');


--end tiansu 20110401











--begin tiansu 20110331
--TSC111013合同金额改为339620.00
--TSC111166合同金额改为330694.00

--修改前4180
update orderdet set includetaxamountto=5334,amountto=5334 where orderno='TSC111013' and item ='25000008';
--修改前6000
update orderdet set includetaxamountto=10000,amountto=10000 where orderno='TSC111166' and item ='39000018';
--end tiansu 20110331



--begin tiansu 20110325


update billmstr set totalbilldetailamount=60817.1,totalbillamount=60817.1 where billno='BIL110140';
update billmstr set totalbilldetailamount=240.0021,totalbillamount=240.0021 where billno='BIL110163';
update billdet set unitprice=240.0021,orderamount=240.0021 where billno='BIL110163';
GO

update orderdet set pricelistdetfrom = 11032,amountfrom=618.00,includetaxamountfrom=618.00 where orderno='STW110061' and item='41000028';
go

--end tiansu 20110325





--begin tiansu 20110324 TSC111014和TSC111012因同事操作有误，出现折扣，请修复一下
update orderdet set amountto=1915.20 where  orderno ='TSC111012' and item ='41000008';
update orderdet set amountto=729.60 where  orderno ='TSC111012' and item ='41000009';
update orderdet set amountto=250.80 where  orderno ='TSC111012' and item ='41000010';
update orderdet set amountto=125.40 where  orderno ='TSC111012' and item ='41000014';
GO
update orderdet set includetaxamountto=amountto*1.17 where  orderno ='TSC111012' ;
GO

update orderdet set amountto=10622 where  orderno ='TSC111014' and item ='22000003';
GO
update orderdet set includetaxamountto=amountto*1.17 where  orderno ='TSC111014' ;
GO
--end tiansu 20110324


--begin tiansu 20110322 价格单变动总价不一致
update orderdet set amountto=7800 ,includetaxamountto=7800 where orderno ='TSC111170' and item='70100003';
GO
--end tiansu 20110322

--beging tiansu 20110318 订单GUH110031物料25000023价格没有带出来
update OrderDet set pricelistdetfrom=12062,amountfrom =126* orderqty,includetaxamountfrom=126* orderqty*1.17    where  id =30227;
GO
--end tiansu 20110318




--begin tiansu 20110314
update actbill set unitprice = 310.6935,billamount=18641.61 where orderno='TSC111045';
GO
update dbo.OrderDet set includetaxamountTo ='18641.61',amountto='15933.00'  where orderno='TSC111045';
GO

update ipmstr set status ='Close' where  ipno='TSK101274';
GO


update actbill set billamount = unitprice where  orderno='DUC100003' and item='34100007';
GO

--订单：SJC110004,第二项库位修改为FDE，谢谢！
update orderdet set locto='FDE' where id=30174;
GO
update orderloctrans  set loc ='FDE' where id=267178;
GO


--我有份订单，之前做的，现在确认其中有二项价格变更过了，能不能你帮我改一下其中的二项，如不方便请回复，谢谢！
--订单号：STW110042,型号：FD100306,单价应为：28.92元；型号：FD100518,单价应为：84.59元。

update orderdet set pricelistdetfrom=12037,includetaxamountfrom= orderqty*28.92,amountfrom=orderqty*28.92 where  id=29750;
update orderdet set pricelistdetfrom=12038,includetaxamountfrom= orderqty*84.59,amountfrom=orderqty*84.59 where  id=29751;
GO


--2.以下是发货确认的问题，这些东西已经收掉了，不能再收了
--   TSK111221	TSK111156   TSK111146   TSK111136   TSK111085   TSK111075   TSK101303   TSK101292   TSK101287   
--   TSK101302   TSK101299   TSK101289   TSK101288   TSK101233   TSK101232   TSK101231   TSK101031	TSK101281
update ipmstr set status ='Close' where  ipno in 
(   'TSK111221',
   'TSK111156',
   'TSK111146',
   'TSK111136',
   'TSK111085',
   'TSK111075',
   'TSK101303',
   'TSK101292',
   'TSK101287',
   'TSK101281',
   'TSK101302',
   'TSK101299',
   'TSK101289',
   'TSK101288',
   'TSK101233',
   'TSK101232',
   'TSK101231',
   'TSK101031'
);
GO


--end tiansu 20110314


--begin tiansu 20110120
update billmstr set totalbilldetailamount=11143.86,totalbillamount=11143.86 where billno='BIL110003'
--end tiansu 20110120



--begin tiansu 20110118 用户修改价格单含税,照成数据不对
update orderdet set includetaxamountfrom = amountFrom where orderno='NOR100003';
GO

update actbill set unitprice=34.76,billamount=billedqty * 34.76,billedamount=billedqty*34.76 where id =4751;
update actbill set unitprice=88,billamount=billedqty * 88,billedamount=billedqty*88 where id =4750;
update actbill set unitprice=47.08,billamount=billedqty * 47.08 where id =4555;
GO

update planbill set unitprice=34.76,planamount=planqty * 34.76,actamount=actqty*34.76 where id =24087;
update planbill set unitprice=88,planamount=planqty * 88,actamount=actqty*88 where id =24086;
update planbill set unitprice=47.08,planamount=planqty *	47.08,actamount=actqty*47.08 where id =23891;
GO

update dbo.BillDet set unitprice=88,orderamount=billedqty * 88 where id=3634;
update dbo.BillDet set unitprice=34.76,orderamount=billedqty * 34.76 where id=3635;
GO

update billmstr set totalbilldetailamount = 787.6,totalbillamount = 787.6 where billno='BIL110056';
GO
--end tiansu 20110118








--begin tiansu 20110111 因为金额小数而照成的偏差没有关闭付款单
update payment set status='Close' where  paymentno='PAY100010';
GO
--end tiansu 20110111


--begin liqiuyun 20101231 新增UsbKey认证
alter table acc_user add UsbKey varchar(50);
alter table acc_user add EnableUsbKey bit;
--end wangxiang 20101231 新增UsbKey认证

--begin wangxiang 20101221 更新订单含税金额，取价格单的是否含税
update orderdet set includetaxamountfrom=d.amountfrom*(1+(1-m.isincludetax)*0.17),isincludetax=m.isincludetax
from orderdet d inner join pricelistdet p on d.pricelistdetfrom=p.id
inner join pricelistmstr m on p.pricelist=m.code
where d.pricelistdetfrom is not null

update orderdet set includetaxamountto=d.amountto*(1+(1-m.isincludetax)*0.17),isincludetax=m.isincludetax
from orderdet d inner join pricelistdet p on d.pricelistdetto=p.id
inner join pricelistmstr m on p.pricelist=m.code
where d.pricelistdetto is not null
--end wangxiang 20101221

--begin tiansu 20101217 修改已经回冲,但是未关闭的账单
update billmstr set status='Close' where BackwashAmount is not null
 and BackwashAmount=TotalBillAmount and TotalBillAmount=TotalBillDetailAmount and status!='Close';
GO
--end tiansu 20101217


--begin tiansu 加审批人和时间,同意人和同意时间,拒绝人和拒绝时间 20101215
alter table OrderMstr add ApprovedUser varchar(50);
alter table OrderMstr add ApprovedDate datetime;
alter table OrderMstr add RejectedUser varchar(50);
alter table OrderMstr add RejectedDate datetime;
alter table ipmstr add ApprovedUser varchar(50);
alter table ipmstr add ApprovedDate datetime;
GO
--end tiansu 20101215



--begin wangxiang 含税金额
alter table OrderDet add IncludeTaxAmountFrom  decimal(18, 8);
alter table OrderDet add IncludeTaxAmountTo  decimal(18, 8);

update orderdet set includetaxamountfrom=d.amountfrom*(1+(1-p.isincludetax)*0.17)
 from orderdet d inner join pricelistdet p on d.pricelistdetfrom=p.id
where d.pricelistdetfrom is not null

update orderdet set includetaxamountto=d.amountto*(1+(1-p.isincludetax)*0.17)
 from orderdet d inner join pricelistdet p on d.pricelistdetto=p.id
where d.pricelistdetto is not null
--end wangxiang

INSERT INTO "ACC_Permission" (PM_Code,PM_Desc,PM_CateCode) VALUES ('AdjustAsn','要货调整','OrderOperation');

--begin tiansu 补全菜单图标

update acc_menu set ImageUrl='~/Images/Nav/OrderGoods.png' where id='165';  --采购订货
update acc_menu set ImageUrl='~/Images/Nav/ItemCategory.png' where id='ItemCategory.166';  --品名
update acc_menu set ImageUrl='~/Images/Nav/ActPayable.png' where id='Menu.ActPayable';  --应付款明细
update acc_menu set ImageUrl='~/Images/Nav/ActReceivable.png' where id='Menu.ActReceivable';  --应收款明细
update acc_menu set ImageUrl='~/Images/Nav/OrderTracking.png' where id='Menu.OrderTracking';  --订单跟踪
update acc_menu set ImageUrl='~/Images/Nav/PayablesAging.png' where id='Menu.PayablesAging';  --应付款账龄
update acc_menu set ImageUrl='~/Images/Nav/POBillPayment.png' where id='Menu.POBillPayment';  --采购账单付款明细
update acc_menu set ImageUrl='~/Images/Nav/PurchasingAnalysis.png' where id='Menu.PurchasingAnalysis';  --采购分析表
update acc_menu set ImageUrl='~/Images/Nav/ReceivableAging.png' where id='Menu.ReceivableAging';  --应收款账龄
update acc_menu set ImageUrl='~/Images/Nav/SalePerformance.png' where id='Menu.SalePerformance';  --销售业绩报表
update acc_menu set ImageUrl='~/Images/Nav/SalesBillPayment.png' where id='Menu.SalesBillPayment';  --销售账单付款明细
update acc_menu set ImageUrl='~/Images/Nav/SalesOrderTracking.png' where id='Menu.SalesOrderTracking';  --销售单跟踪
GO

--end tiansu


--begin tiansu 添加 应付日期/应收日期
alter table BillMstr add  FixtureDate datetime;
alter table IpMstr add DeliverDate datetime;
GO
--end tiansu


--begin tiansu
update ipmstr set billfrom='HAN',billto='FDEIA' where ipno='TSK101001';
GO
--end tiansu


--beging wangxiang
update ipmstr set approvalstatus='Pending' where ordertype='Transfer' and approvalstatus is null
--end wangxiang

--beging tiansu
alter table IpMstr
	add BillFrom	varchar(50)	null,
		BillTo	varchar(50)	null;
GO
--end tiansu



--begin tiansu 20101116隐藏库存调整菜单
update acc_menucommon set isactive=0 where menuid='95';
GO
--end  tiansu 20101116



--beging 盘点
alter table CycleCountMstr add Bins varchar(max);
alter table CycleCountMstr add Items varchar(max);
alter table CycleCountMstr add IsScanHu bit;
alter table CycleCountMstr add StartUser varchar(50);
alter table CycleCountMstr add StartDate datetime;
alter table CycleCountMstr add CompleteUser varchar(50);
alter table CycleCountMstr add CompleteDate datetime;
alter table CycleCountResult add IsProcess bit;

ALTER TABLE [dbo].[CycleCountMstr]  WITH CHECK ADD  CONSTRAINT [FK_CycleCountMstr_ACC_User5] FOREIGN KEY([StartUser])
REFERENCES [dbo].[ACC_User] ([USR_Code]);

ALTER TABLE [dbo].[CycleCountMstr]  WITH CHECK ADD  CONSTRAINT [FK_CycleCountMstr_ACC_User6] FOREIGN KEY([CompleteUser])
REFERENCES [dbo].[ACC_User] ([USR_Code]);

delete from codemstr where code = 'PhysicalCountType' and codevalue = 'SpotCheck';
GO
update ACC_Menu set PageUrl='~/Main.aspx?mid=Inventory.Stocktaking' where Id='94';
update dbo.ACC_Permission set pm_code='~/Main.aspx?mid=Inventory.Stocktaking' where pm_id=541;
GO
--end 








--begin tiansu 20101112 隐藏 库存汇总报表 菜单
update dbo.ACC_MenuCommon set isactive=0 where id=97;
GO
--end tiansu




--begin dx 20101109  添加货运方式
insert into codemstr values ('DeliverType', 'Direct', 12, 0, '直送');
insert into codemstr values ('DeliverType', 'Self', 16, 0, '自提');
--end dx 

--begin dx 20101108  添加辅助字段
alter table party add BoolField1 bit;
alter table party add BoolField2 bit;
alter table ordermstr add BoolField1 bit;
alter table ordermstr add BoolField2 bit;
update party set boolfield1 = 0;
update party set boolfield2 = 0;
update ordermstr set boolfield1 = 0;
update ordermstr set boolfield2 = 0;
--end dx 


--begin tiansu 20101104  销售单折扣精度bug

alter table orderdet alter column DiscountTo decimal(18, 8);
alter table ordermstr alter column DiscountTo decimal(18, 8);
GO

--end tiansu 







--begin tiansu 20101101 
alter view OrderLocTransView
as
SELECT     dbo.OrderLocTrans.Id, dbo.OrderMstr.OrderNo, dbo.OrderMstr.Type, dbo.OrderMstr.Flow, dbo.OrderMstr.Status, dbo.OrderMstr.StartTime, 
                      dbo.OrderMstr.WindowTime, dbo.OrderMstr.PartyFrom, dbo.OrderMstr.PartyTo, dbo.OrderLocTrans.Loc, dbo.OrderDet.Item AS ItemCode, 
                      dbo.Item.Desc1 + dbo.Item.Desc2 AS ItemDesc, dbo.Item.Spec AS ItemSpec, dbo.Item.Brand AS ItemBrand, dbo.OrderDet.Uom, dbo.OrderDet.ReqQty, 
                      dbo.OrderDet.OrderQty, ISNULL(dbo.OrderDet.ShipQty, 0) AS ShipQty, ISNULL(dbo.OrderDet.RecQty, 0) AS RecQty, dbo.OrderLocTrans.Item, 
                      dbo.OrderLocTrans.IOType, dbo.OrderLocTrans.UnitQty, dbo.OrderLocTrans.OrderQty AS PlanQty, ISNULL(dbo.OrderLocTrans.AccumQty, 0) 
                      AS AccumQty, dbo.OrderMstr.ApprovalStatus
FROM         dbo.OrderDet INNER JOIN
                      dbo.OrderMstr ON dbo.OrderDet.OrderNo = dbo.OrderMstr.OrderNo INNER JOIN
                      dbo.OrderLocTrans ON dbo.OrderDet.Id = dbo.OrderLocTrans.OrderDetId INNER JOIN
                      dbo.Item ON dbo.OrderDet.Item = dbo.Item.Code
GO



create view IpView
as
SELECT     dbo.IpDet.OrderLocTransId, SUM(ISNULL(dbo.IpDet.Qty, 0)) AS DeliverQty, MIN(dbo.IpMstr.CreateDate) AS MinDeliverDate, MAX(dbo.IpMstr.CreateDate) 
                      AS MaxDeliverDate
FROM         dbo.IpDet INNER JOIN
                      dbo.IpMstr ON dbo.IpDet.IpNo = dbo.IpMstr.IpNo
GROUP BY dbo.IpDet.OrderLocTransId
GO


alter view SalesOrderTrackingView
as
SELECT     MAX(dbo.OrderDet.Id) AS Id, dbo.OrderMstr.OrderNo, MAX(dbo.OrderDet.Seq) AS Seq, dbo.OrderDet.Item, MIN(dbo.OrderMstr.WindowTime) 
                      AS WindowTime, MIN(dbo.IpView.MinDeliverDate) AS MinDeliverDate, MAX(dbo.IpView.MaxDeliverDate) AS MaxDeliverDate, 
                      SUM(ISNULL(dbo.OrderDet.OrderQty, 0)) AS OrderQty, SUM(ISNULL(dbo.IpView.DeliverQty, 0)) AS DeliverQty, SUM(ISNULL(dbo.OrderDet.OrderQty, 0) 
                      - ISNULL(dbo.IpView.DeliverQty, 0)) AS NoDeliverQty, SUM(ISNULL(dbo.BillView.BillAmount, 0)) AS BillAmount, SUM(ISNULL(dbo.BillView.BilledAmount, 
                      0)) AS BilledAmount, SUM(ISNULL(dbo.BillView.BillAmount, 0) - ISNULL(dbo.BillView.BilledAmount, 0)) AS NoBilledAmount, 
                      MIN(dbo.BillView.MinEffDate) AS EffDate,
                          (SELECT     SUM(ISNULL(OrderQty, 0)) AS OrderedQty
                            FROM          dbo.OrderTracer
                            WHERE      (Code = dbo.OrderMstr.OrderNo) AND (Item = dbo.OrderDet.Item)
                            GROUP BY Code, Item) AS OrderedQty
FROM         dbo.OrderMstr INNER JOIN
                      dbo.OrderDet ON dbo.OrderMstr.OrderNo = dbo.OrderDet.OrderNo INNER JOIN
                      dbo.PartyAddr ON dbo.OrderMstr.BillTo = dbo.PartyAddr.Code AND dbo.PartyAddr.AddrType = 'BillAddr' LEFT OUTER JOIN
                      dbo.OrderLocTrans ON dbo.OrderDet.Id = dbo.OrderLocTrans.OrderDetId AND dbo.OrderLocTrans.IOType = 'Out' LEFT OUTER JOIN
                      dbo.IpView ON dbo.IpView.OrderLocTransId = dbo.OrderLocTrans.Id LEFT OUTER JOIN
                      dbo.BillView ON dbo.BillView.OrderNo = dbo.OrderMstr.OrderNo AND dbo.OrderDet.Item = dbo.BillView.Item
GROUP BY dbo.OrderMstr.OrderNo, dbo.OrderDet.Item
GO

--end tiansu





--beging tiansu 20101029 订单跟踪,销售单跟踪

alter view ReceiptView
as 
SELECT     dbo.ReceiptDet.OrderLocTransId, MIN(dbo.ReceiptMstr.CreateDate) AS MinReceiptDate, MAX(dbo.ReceiptMstr.CreateDate) AS MaxReceiptDate, 
                      SUM(ISNULL(dbo.ReceiptDet.RecQty, 0)) AS RecQty
FROM         dbo.ReceiptDet INNER JOIN
                      dbo.ReceiptMstr ON dbo.ReceiptDet.RecNo = dbo.ReceiptMstr.RecNo
GROUP BY dbo.ReceiptDet.OrderLocTransId
GO


alter view BillView
as
SELECT     dbo.ActBill.OrderNo, dbo.ActBill.Item, SUM(ISNULL(dbo.ActBill.BillAmount, 0)) AS BillAmount, SUM(ISNULL(dbo.ActBill.BilledAmount, 0)) 
                      AS BilledAmount, SUM(ISNULL(dbo.ActBill.BillAmount, 0) - ISNULL(dbo.ActBill.BilledAmount, 0)) AS NoBilledAmount, MIN(dbo.BillMstr.EffDate) 
                      AS MinEffDate, MAX(dbo.BillMstr.EffDate) AS MaxEffDate
FROM         dbo.ActBill LEFT OUTER JOIN
                      dbo.BillDet ON dbo.BillDet.TransId = dbo.ActBill.Id LEFT OUTER JOIN
                      dbo.BillMstr ON dbo.BillDet.BillNo = dbo.BillMstr.BillNo AND dbo.BillMstr.Status <> 'Void' AND dbo.BillMstr.Status <> 'Cancel'
GROUP BY dbo.ActBill.OrderNo, dbo.ActBill.Item
GO



alter view OrderTrackingView
as
SELECT DISTINCT 
                      MAX(dbo.OrderDet.Id) AS Id, dbo.OrderMstr.OrderNo, MAX(dbo.OrderDet.Seq) AS Seq, dbo.OrderDet.Item, MIN(dbo.OrderMstr.WindowTime) 
                      AS WindowTime, MIN(dbo.ReceiptView.MinReceiptDate) AS MinReceiptDate, MAX(dbo.ReceiptView.MaxReceiptDate) AS MaxReceiptDate, 
                      SUM(dbo.OrderDet.OrderQty) AS OrderQty, SUM(dbo.ReceiptView.RecQty) AS RecQty, SUM(ISNULL(dbo.OrderDet.OrderQty, 0) 
                      - ISNULL(dbo.ReceiptView.RecQty, 0)) AS NoRecQty, SUM(dbo.BillView.BillAmount) AS BillAmount, SUM(dbo.BillView.BilledAmount) AS BilledAmount, 
                      SUM(ISNULL(dbo.BillView.BillAmount, 0) - ISNULL(dbo.BillView.BilledAmount, 0)) AS NoBilledAmount, MIN(dbo.BillView.MinEffDate) AS EffDate
FROM         dbo.OrderMstr INNER JOIN
                      dbo.OrderDet ON dbo.OrderMstr.OrderNo = dbo.OrderDet.OrderNo INNER JOIN
                      dbo.PartyAddr ON dbo.OrderMstr.BillFrom = dbo.PartyAddr.Code AND dbo.PartyAddr.AddrType = 'BillAddr' INNER JOIN
                      dbo.OrderLocTrans ON dbo.OrderDet.Id = dbo.OrderLocTrans.OrderDetId AND dbo.OrderLocTrans.IOType = 'In' LEFT OUTER JOIN
                      dbo.ReceiptView ON dbo.ReceiptView.OrderLocTransId = dbo.OrderLocTrans.Id LEFT OUTER JOIN
                      dbo.BillView ON dbo.BillView.OrderNo = dbo.OrderMstr.OrderNo AND dbo.OrderDet.Item = dbo.BillView.Item
GROUP BY dbo.OrderMstr.OrderNo, dbo.OrderDet.Item
GO



alter view SalesOrderTrackingView
as
SELECT     MAX(dbo.OrderDet.Id) AS Id, dbo.OrderMstr.OrderNo, MAX(dbo.OrderDet.Seq) AS Seq, dbo.OrderDet.Item, MIN(dbo.OrderMstr.WindowTime) 
                      AS WindowTime, MIN(dbo.ReceiptView.MinReceiptDate) AS MinReceiptDate, MAX(dbo.ReceiptView.MaxReceiptDate) AS MaxReceiptDate, 
                      SUM(ISNULL(dbo.OrderDet.OrderQty, 0)) AS OrderQty, SUM(ISNULL(dbo.ReceiptView.RecQty, 0)) AS RecQty, SUM(ISNULL(dbo.OrderDet.OrderQty, 0) 
                      - ISNULL(dbo.ReceiptView.RecQty, 0)) AS NoRecQty, SUM(ISNULL(dbo.BillView.BillAmount, 0)) AS BillAmount, SUM(ISNULL(dbo.BillView.BilledAmount, 
                      0)) AS BilledAmount, SUM(ISNULL(dbo.BillView.BillAmount, 0) - ISNULL(dbo.BillView.BilledAmount, 0)) AS NoBilledAmount, 
                      MIN(dbo.BillView.MinEffDate) AS EffDate,
                          (SELECT     SUM(ISNULL(OrderQty, 0)) AS OrderedQty
                            FROM          dbo.OrderTracer
                            WHERE      (Code = dbo.OrderMstr.OrderNo) AND (Item = dbo.OrderDet.Item)
                            GROUP BY Code, Item) AS OrderedQty
FROM         dbo.OrderMstr INNER JOIN
                      dbo.OrderDet ON dbo.OrderMstr.OrderNo = dbo.OrderDet.OrderNo INNER JOIN
                      dbo.PartyAddr ON dbo.OrderMstr.BillTo = dbo.PartyAddr.Code AND dbo.PartyAddr.AddrType = 'BillAddr' LEFT OUTER JOIN
                      dbo.OrderLocTrans ON dbo.OrderDet.Id = dbo.OrderLocTrans.OrderDetId AND dbo.OrderLocTrans.IOType = 'In' LEFT OUTER JOIN
                      dbo.ReceiptView ON dbo.ReceiptView.OrderLocTransId = dbo.OrderLocTrans.Id LEFT OUTER JOIN
                      dbo.BillView ON dbo.BillView.OrderNo = dbo.OrderMstr.OrderNo AND dbo.OrderDet.Item = dbo.BillView.Item
GROUP BY dbo.OrderMstr.OrderNo, dbo.OrderDet.Item
GO


--end tiansu 20101029










--begin tiansu 20101029  销售单跟踪中, 已定数明细

create view OrderDetTracerView
as
SELECT     dbo.OrderDet.Id, dbo.OrderDet.OrderNo, dbo.OrderDet.Item, dbo.OrderDet.RefItemCode, dbo.OrderDet.Seq, dbo.OrderDet.Uom, dbo.OrderDet.UC, 
                      dbo.OrderDet.ReqQty, dbo.OrderDet.OrderQty, dbo.OrderDet.ShipQty, dbo.OrderDet.RecQty, dbo.OrderDet.RejQty, dbo.OrderDet.ScrapQty, 
                      dbo.OrderDet.OrderGrLotSize, dbo.OrderDet.BatchSize, dbo.OrderDet.LocFrom, dbo.OrderDet.LocTo, dbo.OrderDet.BillFrom, dbo.OrderDet.BillTo, 
                      dbo.OrderDet.PriceListFrom, dbo.OrderDet.PriceListTo, dbo.OrderDet.PriceListDetFrom, dbo.OrderDet.PriceListDetTo, dbo.OrderDet.DiscountFrom, 
                      dbo.OrderDet.DiscountTo, dbo.OrderDet.AmountFrom, dbo.OrderDet.AmountTo, dbo.OrderDet.Bom, dbo.OrderDet.HuLotSize, 
                      dbo.OrderDet.BillSettleTerm, dbo.OrderDet.Customer, dbo.OrderDet.PackVol, dbo.OrderDet.PackType, dbo.OrderDet.NeedInspect, dbo.OrderDet.IdMark, 
                      dbo.OrderDet.BarCodeType, dbo.OrderDet.ItemVersion, dbo.OrderDet.OddShipOpt, dbo.OrderDet.CustomerItemCode, dbo.OrderDet.DateField1, 
                      dbo.OrderDet.DateField2, dbo.OrderDet.DateField3, dbo.OrderDet.DateField4, dbo.OrderDet.NumField1, dbo.OrderDet.NumField2, 
                      dbo.OrderDet.NumField3, dbo.OrderDet.NumField4, dbo.OrderDet.NumField5, dbo.OrderDet.NumField6, dbo.OrderDet.NumField7, 
                      dbo.OrderDet.NumField8, dbo.OrderDet.TextField1, dbo.OrderDet.TextField2, dbo.OrderDet.TextField3, dbo.OrderDet.TextField4, 
                      dbo.OrderDet.TextField5, dbo.OrderDet.TextField6, dbo.OrderDet.TextField7, dbo.OrderDet.TextField8, dbo.OrderDet.Brand, dbo.OrderDet.Manufacturer, 
                      dbo.OrderDet.IsIncludeTax, dbo.OrderTracer.Code AS OrderTracerCode, dbo.OrderTracer.Item AS OrderTracerItem
FROM         dbo.OrderDet INNER JOIN
                      dbo.OrderLocTrans ON dbo.OrderLocTrans.OrderDetId = dbo.OrderDet.Id INNER JOIN
                      dbo.OrderTracer ON dbo.OrderTracer.RefOrderLocTransId = dbo.OrderLocTrans.Id
                      
GO


--end tiansu 20101029











--beging tiansu 20101028 付款增加凭证号。

alter table Payment add VoucherNo varchar(50) null;

--end tiansu 20101028


--begin tiansu 20101028 加菜单
--		采购管理->信息->采购账单付款明细
--		销售管理->信息->销售账单付款明细


INSERT acc_menu (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark)  VALUES ('Menu.POBillPayment','Menu.POBillPayment',1,'Menu.POBillPayment','Menu.POBillPayment.Description','采购账单付款明细','~/Main.aspx?mid=Reports.BillPayment__mp--ModuleType-Procurement',1,null,getdate(),null,getdate(),null,null);

set IDENTITY_INSERT ACC_MenuCommon on;

INSERT ACC_MenuCommon(Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser)  VALUES ('308','Menu.POBillPayment',35,3,160,1,getdate(),null,getdate(),null);

set IDENTITY_INSERT ACC_MenuCommon off;

INSERT ACC_Permission VALUES ('~/Main.aspx?mid=Reports.BillPayment__mp--ModuleType-Procurement','采购账单付款明细','Procurement');
GO



INSERT acc_menu (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark)  VALUES ('Menu.SalesBillPayment','Menu.SalesBillPayment',1,'Menu.SalesBillPayment','Menu.SalesBillPayment.Description','销售账单付款明细','~/Main.aspx?mid=Reports.BillPayment__mp--ModuleType-Distribution',1,null,getdate(),null,getdate(),null,null);
set IDENTITY_INSERT ACC_MenuCommon on;

INSERT ACC_MenuCommon(Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser)  VALUES ('309','Menu.SalesBillPayment',75,3,270,1,getdate(),null,getdate(),null);

set IDENTITY_INSERT ACC_MenuCommon off;

INSERT ACC_Permission VALUES ('~/Main.aspx?mid=Reports.BillPayment__mp--ModuleType-Distribution','销售账单付款明细','Distribution');
GO

--end tiansu 20101028













--begin tiansu 20101026  已定数字段取错了

alter view SalesOrderTrackingView
as 
SELECT     MAX(dbo.OrderDet.Id) AS Id, dbo.OrderMstr.OrderNo, MAX(dbo.OrderDet.Seq) AS Seq, dbo.OrderDet.Item, MIN(dbo.OrderMstr.WindowTime) 
                      AS WindowTime, MIN(dbo.ReceiptView.MinReceiptDate) AS MinReceiptDate, MAX(dbo.ReceiptView.MaxReceiptDate) AS MaxReceiptDate, 
                      SUM(ISNULL(dbo.OrderDet.OrderQty, 0)) AS OrderQty, SUM(ISNULL(dbo.OrderDet.RecQty, 0)) AS RecQty, SUM(ISNULL(dbo.OrderDet.OrderQty, 0) 
                      - ISNULL(dbo.OrderDet.RecQty, 0)) AS NoRecQty, SUM(ISNULL(dbo.BillView.BillAmount, 0)) AS BillAmount, SUM(ISNULL(dbo.BillView.BillAmount, 0) 
                      - ISNULL(dbo.BillView.BilledAmount, 0)) AS NoBilledAmount, MIN(dbo.BillView.MinEffDate) AS EffDate,
                          (SELECT     SUM(ISNULL(OrderQty, 0)) AS OrderedQty
                            FROM          dbo.OrderTracer
                            WHERE      (Code = dbo.OrderMstr.OrderNo) AND (Item = dbo.OrderDet.Item)
                            GROUP BY Code, Item) AS OrderedQty
FROM         dbo.OrderMstr INNER JOIN
                      dbo.OrderDet ON dbo.OrderMstr.OrderNo = dbo.OrderDet.OrderNo INNER JOIN
                      dbo.PartyAddr ON dbo.OrderMstr.BillTo = dbo.PartyAddr.Code AND dbo.PartyAddr.AddrType = 'BillAddr' LEFT OUTER JOIN
                      dbo.OrderLocTrans ON dbo.OrderDet.Id = dbo.OrderLocTrans.OrderDetId AND dbo.OrderLocTrans.IOType = 'In' LEFT OUTER JOIN
                      dbo.ReceiptView ON dbo.ReceiptView.OrderLocTransId = dbo.OrderLocTrans.Id LEFT OUTER JOIN
                      dbo.BillView ON dbo.BillView.OrderNo = dbo.OrderMstr.OrderNo AND dbo.OrderDet.Item = dbo.BillView.Item
GROUP BY dbo.PartyAddr.Code, dbo.OrderMstr.OrderNo, dbo.OrderDet.Item

GO

--end tiansu 20101026






--begin tansu 20101026 加菜单
--		采购管理->信息->采购分析表
update ACC_MenuCommon set Seq= 263 where Id=306;
GO


update Acc_Menu set PageUrl = '~/Main.aspx?mid=Reports.SalePerformance__mp--ModuleType-Distribution' where PageUrl='~/Main.aspx?mid=Reports.SalePerformance';
GO
update ACC_Permission set PM_Code = '~/Main.aspx?mid=Reports.SalePerformance__mp--ModuleType-Distribution' where PM_Code='~/Main.aspx?mid=Reports.SalePerformance';
GO

INSERT Acc_Menu (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark)  VALUES ('Menu.PurchasingAnalysis','Menu.PurchasingAnalysis',1,'Menu.PurchasingAnalysis','Menu.PurchasingAnalysis.Description','采购分析表','~/Main.aspx?mid=Reports.SalePerformance__mp--ModuleType-Procurement',1,null,getdate(),null,getdate(),null,null);

set IDENTITY_INSERT ACC_MenuCommon on;

INSERT ACC_MenuCommon(Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser)  VALUES ('307','Menu.PurchasingAnalysis',35,3,150,1,getdate(),null,getdate(),null);

set IDENTITY_INSERT ACC_MenuCommon off;

INSERT ACC_Permission VALUES ('~/Main.aspx?mid=Reports.SalePerformance__mp--ModuleType-Procurement','采购分析表','Procurement');
GO

--end tiansu 


--begin wangxiang 20101021
/****** 对象:  StoredProcedure [dbo].[IpDetTrackView]    修改视图*/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[IpDetTrackView]
AS
SELECT     MAX(dbo.IpDet.Id) AS Id, dbo.OrderMstr.Flow, dbo.IpMstr.CurrOp, dbo.OrderDet.Id AS OrderDetId, SUM(dbo.IpDet.Qty) AS Qty
FROM         dbo.IpDet INNER JOIN
                      dbo.IpMstr ON dbo.IpDet.IpNo = dbo.IpMstr.IpNo INNER JOIN
                      dbo.OrderLocTrans ON dbo.IpDet.OrderLocTransId = dbo.OrderLocTrans.Id INNER JOIN
                      dbo.OrderDet ON dbo.OrderLocTrans.OrderDetId = dbo.OrderDet.Id INNER JOIN
                      dbo.OrderMstr ON dbo.OrderDet.OrderNo = dbo.OrderMstr.OrderNo
WHERE     (dbo.IpMstr.Status = 'In-Process') AND (dbo.IpMstr.Type = 'Nml')
GROUP BY dbo.OrderMstr.Flow, dbo.IpMstr.CurrOp, dbo.OrderDet.Id
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

/****** 对象:  StoredProcedure [dbo].[IpDetView]    修改视图 */

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[IpDetView]
AS
SELECT     MAX(dbo.IpDet.Id) AS Id, dbo.IpDet.IpNo, dbo.OrderDet.Id AS OrderDetailId, SUM(dbo.IpDet.Qty) AS Qty
FROM         dbo.IpDet INNER JOIN
                      dbo.IpMstr ON dbo.IpDet.IpNo = dbo.IpMstr.IpNo INNER JOIN
                      dbo.OrderLocTrans ON dbo.IpDet.OrderLocTransId = dbo.OrderLocTrans.Id INNER JOIN
                      dbo.OrderDet ON dbo.OrderLocTrans.OrderDetId = dbo.OrderDet.Id
WHERE     (dbo.IpMstr.Status = 'In-Process') AND (dbo.IpMstr.Type = 'Nml')
GROUP BY dbo.IpDet.IpNo, dbo.OrderDet.Id
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

--end wangxiang 


--begin tiansu 20101021 应付款账龄/应收款账龄 视图

update Acc_Menu set PageUrl = '~/Main.aspx?mid=Reports.SalesOrderTracking__mp--ModuleType-Distribution' where PageUrl='~/Main.aspx?mid=Reports.SalesOrderTracking';
GO
update ACC_Permission set PM_Code = '~/Main.aspx?mid=Reports.SalesOrderTracking__mp--ModuleType-Distribution' where PM_Code='~/Main.aspx?mid=Reports.SalesOrderTracking';
GO
							 

Alter view ActFundsAgingView
as
SELECT     MAX(dbo.BillMstr.BillNo) AS Id, dbo.BillMstr.TransType, dbo.BillMstr.BillAddr, dbo.BillMstr.Currency, dbo.BillMstr.Status, 
                      SUM(CASE WHEN (datediff(day, effdate, getdate()) <= 30) THEN isnull(TotalBillAmount, 0) - isnull(BackwashAmount, 0) ELSE 0 END) 
                      AS NoBackwashAmount1, SUM(CASE WHEN (datediff(day, effdate, getdate()) <= 30) THEN isnull(TotalBillAmount, 0) ELSE 0 END) 
                      AS TotalBillDetailAmount1, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 30 AND datediff(day, effdate, getdate()) <= 60) 
                      THEN isnull(TotalBillAmount, 0) - isnull(BackwashAmount, 0) ELSE 0 END) AS NoBackwashAmount2, SUM(CASE WHEN (datediff(day, effdate, getdate()) 
                      > 30 AND datediff(day, effdate, getdate()) <= 60) THEN isnull(TotalBillAmount, 0) ELSE 0 END) AS TotalBillDetailAmount2, 
                      SUM(CASE WHEN (datediff(day, effdate, getdate()) > 60 AND datediff(day, effdate, getdate()) <= 90) THEN isnull(TotalBillAmount, 0) 
                      - isnull(BackwashAmount, 0) ELSE 0 END) AS NoBackwashAmount3, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 60 AND datediff(day, effdate, 
                      getdate()) <= 90) THEN isnull(TotalBillAmount, 0) ELSE 0 END) AS TotalBillDetailAmount3, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 90 AND 
                      datediff(day, effdate, getdate()) <= 120) THEN isnull(TotalBillAmount, 0) - isnull(BackwashAmount, 0) ELSE 0 END) AS NoBackwashAmount4, 
                      SUM(CASE WHEN (datediff(day, effdate, getdate()) > 90 AND datediff(day, effdate, getdate()) <= 120) THEN isnull(TotalBillAmount, 0) ELSE 0 END) 
                      AS TotalBillDetailAmount4, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 120 AND datediff(day, effdate, getdate()) <= 150) 
                      THEN isnull(TotalBillAmount, 0) - isnull(BackwashAmount, 0) ELSE 0 END) AS NoBackwashAmount5, SUM(CASE WHEN (datediff(day, effdate, getdate()) 
                      > 120 AND datediff(day, effdate, getdate()) <= 150) THEN isnull(TotalBillAmount, 0) ELSE 0 END) AS TotalBillDetailAmount5, 
                      SUM(CASE WHEN (datediff(day, effdate, getdate()) > 150 AND datediff(day, effdate, getdate()) <= 180) THEN isnull(TotalBillAmount, 0) 
                      - isnull(BackwashAmount, 0) ELSE 0 END) AS NoBackwashAmount6, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 150 AND datediff(day, effdate, 
                      getdate()) <= 180) THEN isnull(TotalBillAmount, 0) ELSE 0 END) AS TotalBillDetailAmount6, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 180 AND
                       datediff(day, effdate, getdate()) <= 210) THEN isnull(TotalBillAmount, 0) - isnull(BackwashAmount, 0) ELSE 0 END) AS NoBackwashAmount7, 
                      SUM(CASE WHEN (datediff(day, effdate, getdate()) > 180 AND datediff(day, effdate, getdate()) <= 210) THEN isnull(TotalBillAmount, 0) ELSE 0 END) 
                      AS TotalBillDetailAmount7, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 210 AND datediff(day, effdate, getdate()) <= 360) 
                      THEN isnull(TotalBillAmount, 0) - isnull(BackwashAmount, 0) ELSE 0 END) AS NoBackwashAmount8, SUM(CASE WHEN (datediff(day, effdate, getdate()) 
                      > 210 AND datediff(day, effdate, getdate()) <= 360) THEN isnull(TotalBillAmount, 0) ELSE 0 END) AS TotalBillDetailAmount8, 
                      SUM(CASE WHEN (datediff(day, effdate, getdate()) > 360) THEN isnull(TotalBillAmount, 0) - isnull(BackwashAmount, 0) ELSE 0 END) 
                      AS NoBackwashAmount9, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 360) THEN isnull(TotalBillAmount, 0) ELSE 0 END) 
                      AS TotalBillDetailAmount9
FROM         dbo.BillMstr INNER JOIN
                      dbo.PartyAddr ON dbo.BillMstr.BillAddr = dbo.PartyAddr.Code AND dbo.BillMstr.Status = 'Submit' INNER JOIN
                      dbo.Party ON dbo.PartyAddr.PartyCode = dbo.Party.Code
WHERE     (dbo.BillMstr.BackwashAmount < dbo.BillMstr.TotalBillAmount) OR
                      (dbo.BillMstr.BackwashAmount IS NULL)
GROUP BY dbo.BillMstr.TransType, dbo.BillMstr.BillAddr, dbo.BillMstr.Currency, dbo.BillMstr.Status


GO


create view BillView
as 
SELECT     dbo.ActBill.OrderNo, dbo.ActBill.Item, dbo.ActBill.Id AS ActBillId, dbo.ActBill.BillAmount, dbo.ActBill.BilledAmount, ISNULL(dbo.ActBill.BillAmount, 0) 
                      - ISNULL(dbo.ActBill.BilledAmount, 0) AS NoBilledAmount, SUM(ISNULL(dbo.BillMstr.TotalBillAmount, 0)) AS TotalBillAmount, 
                      SUM(ISNULL(dbo.BillMstr.TotalBillDetailAmount, 0)) AS TotalBillDetailAmount, MIN(dbo.BillMstr.EffDate) AS MinEffDate, MAX(dbo.BillMstr.EffDate) 
                      AS MaxEffDate
FROM         dbo.ActBill LEFT OUTER JOIN
                      dbo.BillDet ON dbo.BillDet.TransId = dbo.ActBill.Id INNER JOIN
                      dbo.BillMstr ON dbo.BillDet.BillNo = dbo.BillMstr.BillNo AND dbo.BillMstr.Status <> 'Void' AND dbo.BillMstr.Status <> 'Cancel'
GROUP BY dbo.ActBill.OrderNo, dbo.ActBill.Item, dbo.ActBill.Id, dbo.ActBill.BillAmount, dbo.ActBill.BilledAmount

GO




alter view SalesOrderTrackingView
as 
SELECT     MAX(dbo.OrderDet.Id) AS Id, dbo.OrderMstr.OrderNo, MAX(dbo.OrderDet.Seq) AS Seq, dbo.OrderDet.Item, MIN(dbo.OrderMstr.WindowTime) 
                      AS WindowTime, MIN(dbo.ReceiptView.MinReceiptDate) AS MinReceiptDate, MAX(dbo.ReceiptView.MaxReceiptDate) AS MaxReceiptDate, 
                      SUM(ISNULL(dbo.OrderDet.OrderQty, 0)) AS OrderQty, SUM(ISNULL(dbo.OrderDet.RecQty, 0)) AS RecQty, SUM(ISNULL(dbo.OrderDet.OrderQty, 0) 
                      - ISNULL(dbo.OrderDet.RecQty, 0)) AS NoRecQty, SUM(ISNULL(dbo.BillView.BillAmount, 0)) AS BillAmount, SUM(ISNULL(dbo.BillView.BillAmount, 0) 
                      - ISNULL(dbo.BillView.BilledAmount, 0)) AS NoBilledAmount, MIN(dbo.BillView.MinEffDate) AS EffDate,
                          (SELECT     SUM(ISNULL(Qty, 0)) AS Qty
                            FROM          dbo.OrderTracer
                            WHERE      (Code = dbo.OrderMstr.OrderNo) AND (Item = dbo.OrderDet.Item)
                            GROUP BY Code, Item) AS Qty
FROM         dbo.OrderMstr INNER JOIN
                      dbo.OrderDet ON dbo.OrderMstr.OrderNo = dbo.OrderDet.OrderNo INNER JOIN
                      dbo.PartyAddr ON dbo.OrderMstr.BillTo = dbo.PartyAddr.Code AND dbo.PartyAddr.AddrType = 'BillAddr' LEFT OUTER JOIN
                      dbo.OrderLocTrans ON dbo.OrderDet.Id = dbo.OrderLocTrans.OrderDetId AND dbo.OrderLocTrans.IOType = 'In' LEFT OUTER JOIN
                      dbo.ReceiptView ON dbo.ReceiptView.OrderLocTransId = dbo.OrderLocTrans.Id LEFT OUTER JOIN
                      dbo.BillView ON dbo.BillView.OrderNo = dbo.OrderMstr.OrderNo AND dbo.OrderDet.Item = dbo.BillView.Item
GROUP BY dbo.PartyAddr.Code, dbo.OrderMstr.OrderNo, dbo.OrderDet.Item

GO




drop view BillPaymentView;

GO


--end tiansu


--begin  dingxin 20101021

/****** 对象:  StoredProcedure [dbo].[GetNextSequence]    脚本日期: 10/21/2010 13:48:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetNextSequence]
	@CodePrefix varchar(50),
	@NextSequence int OUTPUT
AS
Begin Tran
	Declare @invValue int;
	select  @invValue = IntValue FROM NumCtrl WITH (UPDLOCK, ROWLOCK) where Code = @CodePrefix;
	if @invValue is null
	begin
		if @NextSequence is not null
		begin 
			insert into NumCtrl(Code, IntValue) values (@CodePrefix, @NextSequence + 1);
		end	
		else
		begin
			if left(@CodePrefix, 3) = 'TSC' or left(@CodePrefix, 3) = 'TSK'
			begin
				set @NextSequence = 1001;
				insert into NumCtrl(Code, IntValue) values (@CodePrefix, 1002);
			end
			else
			begin
				set @NextSequence = 1;
				insert into NumCtrl(Code, IntValue) values (@CodePrefix, 2);
			end
		end
	end 
	else
	begin
		if @NextSequence is not null
		begin 
			if @invValue <= @NextSequence
			begin
				update NumCtrl set IntValue = @NextSequence + 1 where Code = @CodePrefix;
			end
		end
		else
		begin
			set @NextSequence = @invValue;
			update NumCtrl set IntValue = @NextSequence + 1 where Code = @CodePrefix;
		end
	end	
Commit tran

--end  dingxin




--begin tiansu 20101020

create view ReceiptView
as
SELECT     dbo.ReceiptDet.OrderLocTransId, MIN(dbo.ReceiptMstr.CreateDate) AS MinReceiptDate, MAX(dbo.ReceiptMstr.CreateDate) AS MaxReceiptDate
FROM         dbo.ReceiptDet INNER JOIN
                      dbo.ReceiptMstr ON dbo.ReceiptDet.RecNo = dbo.ReceiptMstr.RecNo
GROUP BY dbo.ReceiptDet.OrderLocTransId
GO


create view BillPaymentView
as
SELECT     dbo.ActBill.OrderNo, dbo.ActBill.Item, dbo.ActBill.Id AS ActBillId, dbo.ActBill.BillAmount, dbo.ActBill.BilledAmount, ISNULL(dbo.ActBill.BillAmount, 0) 
                      - ISNULL(dbo.ActBill.BilledAmount, 0) AS NoBilledAmount, SUM(ISNULL(dbo.BillMstr.TotalBillAmount, 0)) AS TotalBillAmount, 
                      SUM(ISNULL(dbo.BillMstr.TotalBillDetailAmount, 0)) AS TotalBillDetailAmount, MIN(dbo.BillMstr.EffDate) AS MinEffDate, MAX(dbo.BillMstr.EffDate) 
                      AS MaxEffDate, MIN(dbo.Payment.PaymentDate) AS MinPaymentDate, MAX(dbo.Payment.PaymentDate) AS MaxPaymentDate, 
					  SUM( ISNULL(dbo.BillPayment.BackwashAmount, 0) / (  case when  ISNULL(dbo.BillMstr.TotalBillAmount, 1) = 0 then 1 else ISNULL(dbo.BillMstr.TotalBillAmount, 1) end) * (ISNULL(dbo.BillDet.BilledQty,0) * ISNULL(BillDet.UnitPrice,0) -  ISNULL(dbo.BillDet.Discount,0)) ) AS BackwashAmount 
					,dbo.ActBill.BillAmount - SUM( ISNULL(dbo.BillPayment.BackwashAmount, 0) / (  case when  ISNULL(dbo.BillMstr.TotalBillAmount, 1) = 0 then 1 else ISNULL(dbo.BillMstr.TotalBillAmount, 1) end) * (ISNULL(dbo.BillDet.BilledQty,0) * ISNULL(BillDet.UnitPrice,0) -  ISNULL(dbo.BillDet.Discount,0)) )  AS NoBackwashAmount 
FROM         dbo.ActBill LEFT OUTER JOIN
                      dbo.BillDet ON dbo.BillDet.TransId = dbo.ActBill.Id INNER JOIN
                      dbo.BillMstr ON dbo.BillDet.BillNo = dbo.BillMstr.BillNo AND dbo.BillMstr.Status <> 'Void' AND dbo.BillMstr.Status <> 'Cancel' LEFT OUTER JOIN
                      dbo.BillPayment ON dbo.BillPayment.BillNo = dbo.BillMstr.BillNo LEFT OUTER JOIN
                      dbo.Payment ON dbo.Payment.PaymentNo = dbo.BillPayment.PaymentNo
GROUP BY dbo.ActBill.OrderNo, dbo.ActBill.Item, dbo.ActBill.Id, dbo.ActBill.BillAmount, dbo.ActBill.BilledAmount
GO



Alter VIEW [dbo].[OrderTrackingView]
AS
SELECT DISTINCT 
                      MAX(dbo.OrderDet.Id) AS Id, dbo.OrderMstr.OrderNo, MAX(dbo.OrderDet.Seq) AS Seq, dbo.OrderDet.Item, MIN(dbo.OrderMstr.WindowTime) 
                      AS WindowTime, MIN(dbo.ReceiptView.MinReceiptDate) AS MinReceiptDate, MAX(dbo.ReceiptView.MaxReceiptDate) AS MaxReceiptDate, 
                      SUM(dbo.OrderDet.OrderQty) AS OrderQty, SUM(dbo.OrderDet.RecQty) AS RecQty, SUM(ISNULL(dbo.OrderDet.OrderQty, 0) 
                      - ISNULL(dbo.OrderDet.RecQty, 0)) AS NoRecQty, SUM(dbo.BillPaymentView.BillAmount) AS BillAmount, SUM(ISNULL(dbo.BillPaymentView.BillAmount, 
                      0) - ISNULL(dbo.BillPaymentView.BilledAmount, 0)) AS NoBilledAmount, MIN(dbo.BillPaymentView.MinEffDate) AS EffDate, 
                      SUM(BillPaymentView.BackwashAmount) AS Amount, sum(BillPaymentView.NoBackwashAmount) as NoBackwashAmount,
                      MIN(dbo.BillPaymentView.MinPaymentDate) AS PaymentDate
FROM         dbo.OrderMstr INNER JOIN
                      dbo.OrderDet ON dbo.OrderMstr.OrderNo = dbo.OrderDet.OrderNo INNER JOIN
                      dbo.PartyAddr ON dbo.OrderMstr.BillFrom = dbo.PartyAddr.Code AND dbo.PartyAddr.AddrType = 'BillAddr' LEFT OUTER JOIN
                      dbo.OrderLocTrans ON dbo.OrderDet.Id = dbo.OrderLocTrans.OrderDetId AND dbo.OrderLocTrans.IOType = 'In' LEFT OUTER JOIN
                      dbo.ReceiptView ON dbo.ReceiptView.OrderLocTransId = dbo.OrderLocTrans.Id LEFT OUTER JOIN
                      dbo.BillPaymentView ON dbo.BillPaymentView.OrderNo = dbo.OrderMstr.OrderNo AND dbo.OrderDet.Item = dbo.BillPaymentView.Item
GROUP BY dbo.PartyAddr.Code, dbo.OrderMstr.OrderNo, dbo.OrderDet.Item
GO





Create VIEW [BillDetView]
AS
SELECT DISTINCT 
                      dbo.BillDet.Id, dbo.BillDet.BillNo, dbo.BillDet.TransId, dbo.BillDet.BilledQty, dbo.BillDet.UnitPrice, dbo.BillDet.Currency, dbo.BillDet.Discount, 
                      dbo.BillDet.IsIncludeTax, dbo.BillDet.TaxCode, dbo.BillDet.OrderAmount, dbo.BillDet.DateField1, dbo.BillDet.DateField2, dbo.BillDet.IpNo, 
                      dbo.BillDet.LocFrom, dbo.BillDet.NumField1, dbo.BillDet.NumField2, dbo.BillDet.NumField3, dbo.BillDet.NumField4, dbo.BillDet.RefItemCode, 
                      dbo.BillDet.TextField1, dbo.BillDet.TextField2, dbo.BillDet.TextField3, dbo.BillDet.TextField4, dbo.OrderDet.Item, dbo.OrderDet.OrderNo
FROM         dbo.BillDet INNER JOIN
                      dbo.ActBill ON dbo.BillDet.TransId = dbo.ActBill.Id INNER JOIN
                      dbo.OrderDet ON dbo.ActBill.OrderNo = dbo.OrderDet.OrderNo AND dbo.OrderDet.Item = dbo.ActBill.Item
GO

--end tiansu




--begin tiansu 20101019
--		销售管理->信息->销售单跟踪

INSERT Acc_Menu (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark)  VALUES ('Menu.SalesOrderTracking','Menu.SalesOrderTracking',1,'Menu.SalesOrderTracking','Menu.SalesOrderTracking.Description','销售单跟踪','~/Main.aspx?mid=Reports.SalesOrderTracking__mp--ModuleType-Distribution',1,null,getdate(),null,getdate(),null,null);

set IDENTITY_INSERT ACC_MenuCommon on;

INSERT ACC_MenuCommon(Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser)  VALUES ('306','Menu.SalesOrderTracking',75,3,270,1,getdate(),null,getdate(),null);

set IDENTITY_INSERT ACC_MenuCommon off;

INSERT ACC_Permission VALUES ('~/Main.aspx?mid=Reports.SalesOrderTracking__mp--ModuleType-Distribution','销售单跟踪','Distribution');
GO





Create VIEW [dbo].[SalesOrderTrackingView]
AS
SELECT     MAX(dbo.OrderDet.Id) AS Id, dbo.OrderMstr.OrderNo, MAX(dbo.OrderDet.Seq) AS Seq, dbo.OrderDet.Item, MIN(dbo.OrderMstr.WindowTime) 
                      AS WindowTime, MIN(dbo.ReceiptMstr.CreateDate) AS MinCreateDate, MAX(dbo.ReceiptMstr.CreateDate) AS MaxCreateDate, 
                      SUM(dbo.OrderDet.OrderQty) AS OrderQty, SUM(dbo.OrderDet.RecQty) AS RecQty, SUM(ISNULL(dbo.OrderDet.OrderQty, 0) 
                      - ISNULL(dbo.OrderDet.RecQty, 0)) AS NoRecQty, SUM(dbo.ActBill.BillAmount) AS BillAmount, SUM(ISNULL(dbo.ActBill.BillAmount, 0) 
                      - ISNULL(dbo.ActBill.BilledAmount, 0)) AS NoBilledAmount, SUM(ISNULL(dbo.OrderTracer.Qty, 0)) AS Qty, MIN(dbo.BillMstr.ExtBillNo) AS ExtBillNo, 
                      MIN(dbo.BillMstr.EffDate) AS EffDate, SUM(ISNULL(dbo.BillPayment.BackwashAmount, 0) / ISNULL(dbo.BillMstr.TotalBillAmount, 0) 
                      * ISNULL(dbo.BillDet.OrderAmount, 0)) AS Amount, SUM(ISNULL(dbo.ActBill.BillAmount, 0)) - SUM(ISNULL(dbo.BillPayment.BackwashAmount, 0) 
                      / ISNULL(dbo.BillMstr.TotalBillAmount, 0) * ISNULL(dbo.BillDet.OrderAmount, 0)) AS NoBackwashAmount, MIN(dbo.Payment.PaymentDate) 
                      AS PaymentDate
FROM         dbo.OrderMstr INNER JOIN
                      dbo.OrderDet ON dbo.OrderMstr.OrderNo = dbo.OrderDet.OrderNo INNER JOIN
                      dbo.PartyAddr ON dbo.OrderMstr.BillTo = dbo.PartyAddr.Code AND dbo.PartyAddr.AddrType = 'BillAddr' LEFT OUTER JOIN
                      dbo.OrderTracer ON dbo.OrderTracer.OrderDetId = dbo.OrderDet.Id LEFT OUTER JOIN
                      dbo.OrderLocTrans ON dbo.OrderDet.Id = dbo.OrderLocTrans.OrderDetId INNER JOIN
                      dbo.ReceiptDet ON dbo.ReceiptDet.OrderLocTransId = dbo.OrderLocTrans.Id INNER JOIN
                      dbo.ReceiptMstr ON dbo.ReceiptDet.RecNo = dbo.ReceiptMstr.RecNo LEFT OUTER JOIN
                      dbo.ActBill ON dbo.ActBill.OrderNo = dbo.OrderMstr.OrderNo AND dbo.OrderDet.Item = dbo.ActBill.Item LEFT OUTER JOIN
                      dbo.BillDet ON dbo.BillDet.TransId = dbo.ActBill.Id INNER JOIN
                      dbo.BillMstr ON dbo.BillDet.BillNo = dbo.BillMstr.BillNo LEFT OUTER JOIN
                      dbo.BillPayment ON dbo.BillPayment.BillNo = dbo.BillMstr.BillNo INNER JOIN
                      dbo.Payment ON dbo.Payment.PaymentNo = dbo.BillPayment.PaymentNo
GROUP BY dbo.PartyAddr.Code, dbo.OrderMstr.OrderNo, dbo.OrderDet.Item



--end tiansu






--begin tiansu 20101019
--		采购管理->信息->订单跟踪

INSERT Acc_Menu (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark)  VALUES ('Menu.OrderTracking','Menu.OrderTracking',1,'Menu.OrderTracking','Menu.OrderTracking.Description','订单跟踪','~/Main.aspx?mid=Reports.OrderTracking__mp--ModuleType-Procurement',1,null,getdate(),null,getdate(),null,null);

set IDENTITY_INSERT ACC_MenuCommon on;

INSERT ACC_MenuCommon(Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser)  VALUES ('305','Menu.OrderTracking',35,3,140,1,getdate(),null,getdate(),null);

set IDENTITY_INSERT ACC_MenuCommon off;

INSERT ACC_Permission VALUES ('~/Main.aspx?mid=Reports.OrderTracking__mp--ModuleType-Procurement','订单跟踪','Procurement');
GO



create VIEW [dbo].[OrderTrackingView]
AS
SELECT     MAX(dbo.OrderDet.Id) AS Id, dbo.OrderMstr.OrderNo,max(dbo.OrderDet.Seq) as Seq, dbo.OrderDet.Item, MIN(dbo.OrderMstr.WindowTime) AS WindowTime, 
                      MIN(dbo.ReceiptMstr.CreateDate) AS MinCreateDate, MAX(dbo.ReceiptMstr.CreateDate) AS MaxCreateDate, SUM(dbo.OrderDet.OrderQty) AS OrderQty, 
                      SUM(dbo.OrderDet.RecQty) AS RecQty, SUM(ISNULL(dbo.OrderDet.OrderQty, 0) - ISNULL(dbo.OrderDet.RecQty, 0)) AS NoRecQty, 
                      SUM(dbo.ActBill.BillAmount) AS BillAmount, SUM(ISNULL(dbo.ActBill.BillAmount, 0) - ISNULL(dbo.ActBill.BilledAmount, 0)) AS NoBilledAmount, 
                      MIN(dbo.BillMstr.ExtBillNo) AS ExtBillNo, 
					  min(dbo.BillMstr.EffDate) as EffDate,
SUM(ISNULL(dbo.BillPayment.BackwashAmount, 0) / ISNULL(dbo.BillMstr.TotalBillAmount, 0) 
                      * ISNULL(dbo.BillDet.OrderAmount, 0)) AS Amount, SUM(ISNULL(dbo.ActBill.BillAmount, 0)) - SUM(ISNULL(dbo.BillPayment.BackwashAmount, 0) 
                      / ISNULL(dbo.BillMstr.TotalBillAmount, 0) * ISNULL(dbo.BillDet.OrderAmount, 0)) AS NoBackwashAmount, MIN(dbo.Payment.PaymentDate) 
                      AS PaymentDate
FROM         dbo.OrderMstr INNER JOIN
                      dbo.OrderDet ON dbo.OrderMstr.OrderNo = dbo.OrderDet.OrderNo INNER JOIN 
                      dbo.PartyAddr ON dbo.OrderMstr.BillFrom= dbo.PartyAddr.Code AND dbo.PartyAddr.AddrType='BillAddr' LEFT OUTER JOIN 
                      dbo.OrderLocTrans ON dbo.OrderDet.Id = dbo.OrderLocTrans.OrderDetId INNER JOIN
                      dbo.ReceiptDet ON dbo.ReceiptDet.OrderLocTransId = dbo.OrderLocTrans.Id INNER JOIN
                      dbo.ReceiptMstr ON dbo.ReceiptDet.RecNo = dbo.ReceiptMstr.RecNo LEFT OUTER JOIN
                      dbo.ActBill ON dbo.ActBill.OrderNo = dbo.OrderMstr.OrderNo AND dbo.OrderDet.Item = dbo.ActBill.Item LEFT OUTER JOIN
                      dbo.BillDet ON dbo.BillDet.TransId = dbo.ActBill.Id INNER JOIN
                      dbo.BillMstr ON dbo.BillDet.BillNo = dbo.BillMstr.BillNo LEFT OUTER JOIN
                      dbo.BillPayment ON dbo.BillPayment.BillNo = dbo.BillMstr.BillNo INNER JOIN
                      dbo.Payment ON dbo.Payment.PaymentNo = dbo.BillPayment.PaymentNo
GROUP BY dbo.PartyAddr.Code,dbo.OrderMstr.OrderNo, dbo.OrderDet.Item
GO













--金额取折扣后金额				    
alter VIEW ActFundsAgingView AS 
	 select     MAX(dbo.BillMstr.BillNo) AS Id, dbo.BillMstr.TransType, dbo.BillMstr.BillAddr, dbo.BillMstr.Currency, dbo.BillMstr.Status, 
                      SUM(CASE WHEN (datediff(day, effdate, getdate()) <= 30) THEN isnull(TotalBillAmount, 0) - isnull(BackwashAmount, 0) ELSE 0 END) 
                      AS NoBackwashAmount1, SUM(CASE WHEN (datediff(day, effdate, getdate()) <= 30) THEN isnull(TotalBillAmount, 0) ELSE 0 END) 
                      AS TotalBillDetailAmount1, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 30 AND datediff(day, effdate, getdate()) <= 60) 
                      THEN isnull(TotalBillAmount, 0) - isnull(BackwashAmount, 0) ELSE 0 END) AS NoBackwashAmount2, SUM(CASE WHEN (datediff(day, effdate, 
                      getdate()) > 30 AND datediff(day, effdate, getdate()) <= 60) THEN isnull(TotalBillAmount, 0) ELSE 0 END) AS TotalBillDetailAmount2, 
                      SUM(CASE WHEN (datediff(day, effdate, getdate()) > 60 AND datediff(day, effdate, getdate()) <= 90) THEN isnull(TotalBillAmount, 0) 
                      - isnull(BackwashAmount, 0) ELSE 0 END) AS NoBackwashAmount3, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 60 AND datediff(day, effdate, 
                      getdate()) <= 90) THEN isnull(TotalBillAmount, 0) ELSE 0 END) AS TotalBillDetailAmount3, SUM(CASE WHEN (datediff(day, effdate, getdate()) 
                      > 90 AND datediff(day, effdate, getdate()) <= 120) THEN isnull(TotalBillAmount, 0) - isnull(BackwashAmount, 0) ELSE 0 END) 
                      AS NoBackwashAmount4, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 90 AND datediff(day, effdate, getdate()) <= 120) 
                      THEN isnull(TotalBillAmount, 0) ELSE 0 END) AS TotalBillDetailAmount4, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 120 AND 
                      datediff(day, effdate, getdate()) <= 150) THEN isnull(TotalBillAmount, 0) - isnull(BackwashAmount, 0) ELSE 0 END) AS NoBackwashAmount5, 
                      SUM(CASE WHEN (datediff(day, effdate, getdate()) > 120 AND datediff(day, effdate, getdate()) <= 150) THEN isnull(TotalBillAmount, 0) ELSE 0 END) 
                      AS TotalBillDetailAmount5, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 150 AND datediff(day, effdate, getdate()) <= 180) 
                      THEN isnull(TotalBillAmount, 0) - isnull(BackwashAmount, 0) ELSE 0 END) AS NoBackwashAmount6, SUM(CASE WHEN (datediff(day, effdate, 
                      getdate()) > 150 AND datediff(day, effdate, getdate()) <= 180) THEN isnull(TotalBillAmount, 0) ELSE 0 END) AS TotalBillDetailAmount6, 
                      SUM(CASE WHEN (datediff(day, effdate, getdate()) > 180 AND datediff(day, effdate, getdate()) <= 210) THEN isnull(TotalBillAmount, 0) 
                      - isnull(BackwashAmount, 0) ELSE 0 END) AS NoBackwashAmount7, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 180 AND datediff(day, effdate, 
                      getdate()) <= 210) THEN isnull(TotalBillAmount, 0) ELSE 0 END) AS TotalBillDetailAmount7, SUM(CASE WHEN (datediff(day, effdate, getdate()) 
                      > 210 AND datediff(day, effdate, getdate()) <= 360) THEN isnull(TotalBillAmount, 0) - isnull(BackwashAmount, 0) ELSE 0 END) 
                      AS NoBackwashAmount8, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 210 AND datediff(day, effdate, getdate()) <= 360) 
                      THEN isnull(TotalBillAmount, 0) ELSE 0 END) AS TotalBillDetailAmount8, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 360) 
                      THEN isnull(TotalBillAmount, 0) - isnull(BackwashAmount, 0) ELSE 0 END) AS NoBackwashAmount9, SUM(CASE WHEN (datediff(day, effdate, 
                      getdate()) > 360) THEN isnull(TotalBillAmount, 0) ELSE 0 END) AS TotalBillDetailAmount9
FROM         dbo.BillMstr INNER JOIN
                      dbo.PartyAddr ON dbo.BillMstr.BillAddr = dbo.PartyAddr.Code INNER JOIN
                      dbo.Party ON dbo.PartyAddr.PartyCode = dbo.Party.Code
WHERE     (dbo.BillMstr.BackwashAmount < dbo.BillMstr.TotalBillDetailAmount) OR
                      (dbo.BillMstr.BackwashAmount IS NULL)
GROUP BY dbo.BillMstr.TransType, dbo.BillMstr.BillAddr, dbo.BillMstr.Currency, dbo.BillMstr.Status		    
GO

--end tiansu




--begin tiansu 20101015
--		销售管理->信息->销售业绩报表
alter table OrderDet add IsIncludeTax bit null;
GO
update OrderDet set IsIncludeTax=1;
GO


INSERT Acc_Menu (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark)  VALUES ('Menu.SalePerformance','Menu.SalePerformance',1,'Menu.SalePerformance','Menu.SalePerformance.Description','销售业绩报表','~/Main.aspx?mid=Reports.SalePerformance',1,null,getdate(),null,getdate(),null,null);

set IDENTITY_INSERT ACC_MenuCommon on;

INSERT ACC_MenuCommon(Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser)  VALUES ('304','Menu.SalePerformance',75,3,265,1,getdate(),null,getdate(),null);

set IDENTITY_INSERT ACC_MenuCommon off;

INSERT ACC_Permission VALUES ('~/Main.aspx?mid=Reports.SalePerformance','销售业绩报表','Distribution');
GO

--end tiansu





--begin tiansu 20101013 加菜单
--		采购管理->信息->应付款明细
--					  ->应付款账龄
--		销售管理->信息->应收款明细
--				      ->应收款账龄


INSERT acc_menu (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark)  VALUES ('Menu.ActPayable','Menu.ActPayable',1,'Menu.ActPayable','Menu.ActPayable.Description','应付款明细','~/Main.aspx?mid=Reports.ActFunds__mp--ModuleType-Procurement',1,null,getdate(),null,getdate(),null,null);
INSERT acc_menu (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark)  VALUES ('Menu.PayablesAging','Menu.PayablesAging',1,'Menu.PayablesAging','Menu.PayablesAging.Description','应付款账龄','~/Main.aspx?mid=Reports.ActFundsAging__mp--ModuleType-Procurement',1,null,getdate(),null,getdate(),null,null);

set IDENTITY_INSERT ACC_MenuCommon on;

INSERT ACC_MenuCommon(Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser)  VALUES ('300','Menu.ActPayable',35,3,120,1,getdate(),null,getdate(),null);
INSERT ACC_MenuCommon(Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser)  VALUES ('301','Menu.PayablesAging',35,3,130,1,getdate(),null,getdate(),null);

set IDENTITY_INSERT ACC_MenuCommon off;

INSERT ACC_Permission VALUES ('~/Main.aspx?mid=Reports.ActFunds__mp--ModuleType-Procurement','应付款明细','Procurement');
INSERT ACC_Permission VALUES ('~/Main.aspx?mid=Reports.ActFundsAging__mp--ModuleType-Procurement','应付款账龄','Procurement');
GO



INSERT acc_menu (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark)  VALUES ('Menu.ActReceivable','Menu.ActReceivable',1,'Menu.ActReceivable','Menu.ActReceivable.Description','应收款明细','~/Main.aspx?mid=Reports.ActFunds__mp--ModuleType-Distribution',1,null,getdate(),null,getdate(),null,null);
INSERT acc_menu (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark)  VALUES ('Menu.ReceivableAging','Menu.ReceivableAging',1,'Menu.ReceivableAging','Menu.ReceivableAging.Description','应收款账龄','~/Main.aspx?mid=Reports.ActFundsAging__mp--ModuleType-Distribution',1,null,getdate(),null,getdate(),null,null);

set IDENTITY_INSERT ACC_MenuCommon on;

INSERT ACC_MenuCommon(Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser)  VALUES ('302','Menu.ActReceivable',75,3,251,1,getdate(),null,getdate(),null);
INSERT ACC_MenuCommon(Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser)  VALUES ('303','Menu.ReceivableAging',75,3,261,1,getdate(),null,getdate(),null);

set IDENTITY_INSERT ACC_MenuCommon off;

INSERT ACC_Permission VALUES ('~/Main.aspx?mid=Reports.ActFunds__mp--ModuleType-Distribution','应收款明细','Distribution');
INSERT ACC_Permission VALUES ('~/Main.aspx?mid=Reports.ActFundsAging__mp--ModuleType-Distribution','应收款账龄','Distribution');
GO




alter table BillMstr add EffDate datetime 
					    ,SubmitDate datetime
					    ,SubmitUser varchar(50)
					    ,CloseDate datetime
					    ,CloseUser varchar(50)
					    ,ScrapDate datetime
					    ,ScrapUser varchar(50);
					    
GO


					    
CREATE VIEW ActFundsAgingView AS 
	SELECT     MAX(dbo.BillMstr.BillNo) AS Id, dbo.BillMstr.TransType, dbo.BillMstr.BillAddr, dbo.BillMstr.Currency, dbo.BillMstr.Status, 
                      SUM(CASE WHEN (datediff(day, effdate, getdate()) <= 30) THEN isnull(TotalBillDetailAmount, 0) - isnull(BackwashAmount, 0) ELSE 0 END) 
                      AS NoBackwashAmount1, SUM(CASE WHEN (datediff(day, effdate, getdate()) <= 30) THEN isnull(TotalBillDetailAmount, 0) ELSE 0 END) 
                      AS TotalBillDetailAmount1, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 30 AND datediff(day, effdate, getdate()) <= 60) 
                      THEN isnull(TotalBillDetailAmount, 0) - isnull(BackwashAmount, 0) ELSE 0 END) AS NoBackwashAmount2, SUM(CASE WHEN (datediff(day, effdate, 
                      getdate()) > 30 AND datediff(day, effdate, getdate()) <= 60) THEN isnull(TotalBillDetailAmount, 0) ELSE 0 END) AS TotalBillDetailAmount2, 
                      SUM(CASE WHEN (datediff(day, effdate, getdate()) > 60 AND datediff(day, effdate, getdate()) <= 90) THEN isnull(TotalBillDetailAmount, 0) 
                      - isnull(BackwashAmount, 0) ELSE 0 END) AS NoBackwashAmount3, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 60 AND datediff(day, effdate, 
                      getdate()) <= 90) THEN isnull(TotalBillDetailAmount, 0) ELSE 0 END) AS TotalBillDetailAmount3, SUM(CASE WHEN (datediff(day, effdate, getdate()) 
                      > 90 AND datediff(day, effdate, getdate()) <= 120) THEN isnull(TotalBillDetailAmount, 0) - isnull(BackwashAmount, 0) ELSE 0 END) 
                      AS NoBackwashAmount4, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 90 AND datediff(day, effdate, getdate()) <= 120) 
                      THEN isnull(TotalBillDetailAmount, 0) ELSE 0 END) AS TotalBillDetailAmount4, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 120 AND 
                      datediff(day, effdate, getdate()) <= 150) THEN isnull(TotalBillDetailAmount, 0) - isnull(BackwashAmount, 0) ELSE 0 END) AS NoBackwashAmount5, 
                      SUM(CASE WHEN (datediff(day, effdate, getdate()) > 120 AND datediff(day, effdate, getdate()) <= 150) THEN isnull(TotalBillDetailAmount, 0) ELSE 0 END) 
                      AS TotalBillDetailAmount5, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 150 AND datediff(day, effdate, getdate()) <= 180) 
                      THEN isnull(TotalBillDetailAmount, 0) - isnull(BackwashAmount, 0) ELSE 0 END) AS NoBackwashAmount6, SUM(CASE WHEN (datediff(day, effdate, 
                      getdate()) > 150 AND datediff(day, effdate, getdate()) <= 180) THEN isnull(TotalBillDetailAmount, 0) ELSE 0 END) AS TotalBillDetailAmount6, 
                      SUM(CASE WHEN (datediff(day, effdate, getdate()) > 180 AND datediff(day, effdate, getdate()) <= 210) THEN isnull(TotalBillDetailAmount, 0) 
                      - isnull(BackwashAmount, 0) ELSE 0 END) AS NoBackwashAmount7, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 180 AND datediff(day, effdate, 
                      getdate()) <= 210) THEN isnull(TotalBillDetailAmount, 0) ELSE 0 END) AS TotalBillDetailAmount7, SUM(CASE WHEN (datediff(day, effdate, getdate()) 
                      > 210 AND datediff(day, effdate, getdate()) <= 360) THEN isnull(TotalBillDetailAmount, 0) - isnull(BackwashAmount, 0) ELSE 0 END) 
                      AS NoBackwashAmount8, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 210 AND datediff(day, effdate, getdate()) <= 360) 
                      THEN isnull(TotalBillDetailAmount, 0) ELSE 0 END) AS TotalBillDetailAmount8, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 360) 
                      THEN isnull(TotalBillDetailAmount, 0) - isnull(BackwashAmount, 0) ELSE 0 END) AS NoBackwashAmount9, SUM(CASE WHEN (datediff(day, effdate, 
                      getdate()) > 360) THEN isnull(TotalBillDetailAmount, 0) ELSE 0 END) AS TotalBillDetailAmount9
FROM         dbo.BillMstr INNER JOIN
                      dbo.PartyAddr ON dbo.BillMstr.BillAddr = dbo.PartyAddr.Code INNER JOIN
                      dbo.Party ON dbo.PartyAddr.PartyCode = dbo.Party.Code
WHERE     (dbo.BillMstr.BackwashAmount < dbo.BillMstr.TotalBillDetailAmount) OR
                      (dbo.BillMstr.BackwashAmount IS NULL)
GROUP BY dbo.BillMstr.TransType, dbo.BillMstr.BillAddr, dbo.BillMstr.Currency, dbo.BillMstr.Status		    
GO



update  billmstr set ScrapDate=LastModifyDate,ScrapUser = LastModifyUser   where status='Void' ;
GO

update  billmstr set CloseDate=LastModifyDate,CloseUser = LastModifyUser   where status='Close' ;
GO

update  billmstr set SubmitDate=LastModifyDate,SubmitUser = LastModifyUser  where status='Submit' ;
GO

update billmstr set effdate=SubmitDate;
GO


--end tiansu 20101013


--begin tiansu 20101013 修改  采购未开票明细 和 采购未开票账龄 

ALTER VIEW [dbo].[ActBillView]
AS
SELECT     MAX(Id) AS Id, OrderNo, RecNo, ExtRecNo, TransType, BillAddr, Item, Uom, UC, EffDate
, SUM(BillQty - ISNULL(BilledQty, 0)) AS Qty
,SUM((BillQty - ISNULL(BilledQty, 0))* UnitPrice) AS Amount
,Currency
FROM         dbo.ActBill
WHERE     (BillQty > 0) AND (BillQty > BilledQty) OR
                      (BillQty < 0) AND (BillQty < BilledQty)
GROUP BY OrderNo, RecNo, ExtRecNo, TransType, BillAddr, Item, Uom, UC, EffDate,Currency
GO



ALTER VIEW [dbo].[BillAgingView]
AS
SELECT     MAX(Id) AS ID, TransType, BillAddr, Item, Uom, UC,Currency, SUM(CASE WHEN (datediff(day, effdate, getdate()) <= 30) THEN billqty - isnull(billedqty, 0) 
                      ELSE 0 END) AS Qty1, SUM(CASE WHEN (datediff(day, effdate, getdate()) <= 30) THEN ((billqty - isnull(billedqty, 0)) * UnitPrice) ELSE 0 END) 
                      AS Amount1, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 30 AND datediff(day, effdate, getdate()) <= 60) THEN billqty - isnull(billedqty, 0) 
                      ELSE 0 END) AS Qty2, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 30 AND datediff(day, effdate, getdate()) <= 60) THEN ((billqty - isnull(billedqty,
                       0)) * UnitPrice) ELSE 0 END) AS Amount2, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 60 AND datediff(day, effdate, getdate()) <= 90) 
                      THEN billqty - isnull(billedqty, 0) ELSE 0 END) AS Qty3, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 60 AND datediff(day, effdate, getdate()) 
                      <= 90) THEN ((billqty - isnull(billedqty, 0)) * UnitPrice) ELSE 0 END) AS Amount3, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 90 AND 
                      datediff(day, effdate, getdate()) <= 120) THEN billqty - isnull(billedqty, 0) ELSE 0 END) AS Qty4, SUM(CASE WHEN (datediff(day, effdate, getdate()) 
                      > 90 AND datediff(day, effdate, getdate()) <= 120) THEN ((billqty - isnull(billedqty, 0)) * UnitPrice) ELSE 0 END) AS Amount4, 
                      SUM(CASE WHEN (datediff(day, effdate, getdate()) > 120 AND datediff(day, effdate, getdate()) <= 150) THEN billqty - isnull(billedqty, 0) ELSE 0 END) 
                      AS Qty5, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 120 AND datediff(day, effdate, getdate()) <= 150) THEN ((billqty - isnull(billedqty, 0)) 
                      * UnitPrice) ELSE 0 END) AS Amount5, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 150 AND datediff(day, effdate, getdate()) <= 180) 
                      THEN billqty - isnull(billedqty, 0) ELSE 0 END) AS Qty6, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 150 AND datediff(day, effdate, getdate()) 
                      <= 180) THEN ((billqty - isnull(billedqty, 0)) * UnitPrice) ELSE 0 END) AS Amount6, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 180 AND 
                      datediff(day, effdate, getdate()) <= 210) THEN billqty - isnull(billedqty, 0) ELSE 0 END) AS Qty7, SUM(CASE WHEN (datediff(day, effdate, getdate()) 
                      > 180 AND datediff(day, effdate, getdate()) <= 210) THEN ((billqty - isnull(billedqty, 0)) * UnitPrice) ELSE 0 END) AS Amount7, 
                      SUM(CASE WHEN (datediff(day, effdate, getdate()) > 210 AND datediff(day, effdate, getdate()) <= 360) THEN billqty - isnull(billedqty, 0) ELSE 0 END) 
                      AS Qty8, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 210 AND datediff(day, effdate, getdate()) <= 360) THEN ((billqty - isnull(billedqty, 0)) 
                      * UnitPrice) ELSE 0 END) AS Amount8, SUM(CASE WHEN (datediff(day, effdate, getdate()) > 360) THEN billqty - isnull(billedqty, 0) ELSE 0 END) AS Qty9, 
                      SUM(CASE WHEN (datediff(day, effdate, getdate()) > 360) THEN ((billqty - isnull(billedqty, 0)) * UnitPrice) ELSE 0 END) AS Amount9
FROM         dbo.ActBill
WHERE     (BillQty > 0) AND (BillQty > BilledQty) OR
                      (BillQty < 0) AND (BillQty < BilledQty)
GROUP BY TransType, BillAddr, Item, Uom, UC,Currency
GO


--end tiansu 20101013 







INSERT INTO "CodeMstr" (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('RoundUpOption','1',10,1,'向上圆整');
INSERT INTO "CodeMstr" (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('RoundUpOption','0',20,0,'不圆整');
INSERT INTO "CodeMstr" (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('RoundUpOption','-1',30,0,'向下圆整');

Drop view [OrderLocTransView];
GO

/****** 对象:  View [dbo].[OrderLocTransView]    脚本日期: 10/12/2010 09:31:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[OrderLocTransView]
AS
SELECT     dbo.OrderLocTrans.Id, dbo.OrderMstr.OrderNo, dbo.OrderMstr.Type, dbo.OrderMstr.Flow, dbo.OrderMstr.Status, dbo.OrderMstr.StartTime, dbo.OrderMstr.WindowTime, 
                      dbo.OrderMstr.PartyFrom, dbo.OrderMstr.PartyTo, dbo.OrderLocTrans.Loc, dbo.OrderDet.Item AS ItemCode, dbo.Item.Desc1 + dbo.Item.Desc2 AS ItemDesc, 
                      dbo.OrderDet.Uom, dbo.OrderDet.ReqQty, dbo.OrderDet.OrderQty, ISNULL(dbo.OrderDet.ShipQty, 0) AS ShipQty, ISNULL(dbo.OrderDet.RecQty, 0) AS RecQty, 
                      dbo.OrderLocTrans.Item, dbo.OrderLocTrans.IOType, dbo.OrderLocTrans.UnitQty, dbo.OrderLocTrans.OrderQty AS PlanQty, ISNULL(dbo.OrderLocTrans.AccumQty, 0) 
                      AS AccumQty, dbo.OrderMstr.ApprovalStatus
FROM         dbo.OrderDet INNER JOIN
                      dbo.OrderMstr ON dbo.OrderDet.OrderNo = dbo.OrderMstr.OrderNo INNER JOIN
                      dbo.OrderLocTrans ON dbo.OrderDet.Id = dbo.OrderLocTrans.OrderDetId INNER JOIN
                      dbo.Item ON dbo.OrderDet.Item = dbo.Item.Code

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[31] 4[45] 2[16] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "OrderDet"
            Begin Extent = 
               Top = 22
               Left = 305
               Bottom = 158
               Right = 466
            End
            DisplayFlags = 280
            TopColumn = 20
         End
         Begin Table = "OrderMstr"
            Begin Extent = 
               Top = 18
               Left = 81
               Bottom = 164
               Right = 247
            End
            DisplayFlags = 280
            TopColumn = 101
         End
         Begin Table = "OrderLocTrans"
            Begin Extent = 
               Top = 31
               Left = 506
               Bottom = 155
               Right = 670
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Item"
            Begin Extent = 
               Top = 38
               Left = 718
               Bottom = 153
               Right = 874
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 1410
         Table = 1395
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'OrderLocTransView'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'OrderLocTransView'
GO


insert into codemstr values('FlowStrategy', 'TRD', 70, 0,'贸易')
go

drop view LeanEngineView
go
/****** 对象:  View [dbo].[LeanEngineView]    脚本日期: 09/29/2010 10:10:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[LeanEngineView]
AS
SELECT     dbo.FlowView.FlowDetId, dbo.FlowView.Flow, dbo.FlowView.IsAutoCreate, dbo.FlowView.LocFrom, dbo.FlowView.LocTo, dbo.FlowDet.Item, 
                      dbo.FlowDet.Uom, dbo.FlowDet.UC, dbo.FlowDet.HuLotSize, dbo.FlowDet.Bom, dbo.FlowDet.SafeStock, dbo.FlowDet.MaxStock, 
                      dbo.FlowDet.MinLotSize, dbo.FlowDet.OrderLotSize, dbo.FlowDet.BatchSize, dbo.FlowDet.RoundUpOpt, dbo.FlowMstr.Type, dbo.FlowMstr.PartyFrom, 
                      dbo.FlowMstr.PartyTo, dbo.FlowMstr.FlowStrategy, dbo.FlowMstr.LeadTime, dbo.FlowMstr.EmTime, dbo.FlowMstr.MaxCirTime, 
                      dbo.FlowMstr.WinTime1, dbo.FlowMstr.WinTime2, dbo.FlowMstr.WinTime3, dbo.FlowMstr.WinTime4, dbo.FlowMstr.WinTime5, 
                      dbo.FlowMstr.WinTime6, dbo.FlowMstr.WinTime7, dbo.FlowMstr.NextOrderTime, dbo.FlowMstr.NextWinTime, dbo.FlowMstr.WeekInterval, 
                      dbo.FlowDet.ExtraDmdSource
FROM         dbo.FlowView INNER JOIN
                      dbo.FlowDet ON dbo.FlowView.FlowDetId = dbo.FlowDet.Id INNER JOIN
                      dbo.FlowMstr ON dbo.FlowDet.Flow = dbo.FlowMstr.Code

GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "FlowView"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 166
               Right = 184
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "FlowDet"
            Begin Extent = 
               Top = 6
               Left = 222
               Bottom = 191
               Right = 396
            End
            DisplayFlags = 280
            TopColumn = 36
         End
         Begin Table = "FlowMstr"
            Begin Extent = 
               Top = 6
               Left = 434
               Bottom = 211
               Right = 618
            End
            DisplayFlags = 280
            TopColumn = 69
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'LeanEngineView'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'LeanEngineView'
go

alter table FlowDet add ExtraDmdSource varchar(255);

INSERT INTO "ACC_Permission" (PM_Code, PM_Desc, PM_CateCode) VALUES ('~/Main.aspx?mid=MasterData.ItemCategory', '产品类', 'MasterData');

--begin tiansu 20100928 加 采购管理->供应商 菜单

update codemstr	set Desc1='发货单' where code='AsnTemplate' and codevalue='DeliveryNote.xls' and seq=10 and isdefault=1 and Desc1='送货单';

delete from codemstr where code='OrderTemplate';
delete from codemstr where code='AsnTemplate' and codevalue='ASN.xls' and seq=20 and isdefault=0 and Desc1='出门证';

INSERT INTO "CodeMstr" (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('OrderTemplate','RequisitionOrder.xls',10,1,'要货单');
INSERT INTO "CodeMstr" (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('OrderTemplate','Contract.xls',20,0,'销售合同');
INSERT INTO "CodeMstr" (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('OrderTemplate','Purchase.xls',30,0,'订单');
INSERT INTO "CodeMstr" (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('OrderTemplate','PurchaseAbroad.xls',40,0,'订单(海外)');

update acc_menu set pageurl='~/Main.aspx?mid=MasterData.Supplier' where id='142';
set IDENTITY_INSERT ACC_MenuCommon on;
INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (142,'142','40',2,142,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
set IDENTITY_INSERT ACC_MenuCommon off;
GO
--end tiansu 20100928 


--begin dingxin 20100927 增加采购、销售付款权限
INSERT INTO "ACC_Permission" (PM_Code, PM_Desc, PM_CateCode) VALUES ('~/Main.aspx?mid=Finance.Payment__mp--ModuleType-PO', '采购付款', 'Procurement');
INSERT INTO "ACC_Permission" (PM_Code, PM_Desc, PM_CateCode) VALUES ('~/Main.aspx?mid=Finance.Payment__mp--ModuleType-SO', '销售付款', 'Distribution');
GO
--end dingxin

--begin tiansu 20100927 加 生产管理->BOM 菜单
set IDENTITY_INSERT ACC_MenuCommon on;
INSERT INTO "dbo"."ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (61,'61','60',3,183,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null)
set IDENTITY_INSERT ACC_MenuCommon off;
--end tiansu 20100927


--begin dingxin 20100926 增加订单追踪表
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OrderTracer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderDetId] [int] NOT NULL,
	[TracerType] [varchar](50) NOT NULL,
	[Code] [varchar](50) NOT NULL,
	[Item] [varchar](50) NOT NULL,
	[ReqTime] [datetime] NOT NULL,
	[OrderQty] [decimal](18, 8) NOT NULL CONSTRAINT [DF_OrderTracer_OrderQty]  DEFAULT ((0)),
	[AccumQty] [decimal](18, 8) NOT NULL CONSTRAINT [DF_OrderTracer_AccumQty]  DEFAULT ((0)),
	[Qty] [decimal](18, 8) NOT NULL CONSTRAINT [DF_OrderTracer_Qty]  DEFAULT ((0)),
	[RefOrderLocTransId] [int] NOT NULL CONSTRAINT [DF_OrderTracer_RefOrderLocTransId]  DEFAULT ((0)),
	[Memo] [varchar](255) NULL,
 CONSTRAINT [PK_OrderTracer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[OrderTracer]  WITH CHECK ADD  CONSTRAINT [FK_OrderTracer_OrderDet] FOREIGN KEY([OrderDetId])
REFERENCES [dbo].[OrderDet] ([Id])
GO
--end dingxin

--begin dingxin 20100926 修改菜单
INSERT ACC_Permission VALUES ('~/Main.aspx?mid=Order.OrderGoods','采购订货','Procurement');
INSERT acc_menu VALUES ('165','Menu.OrderGoods.165',1,'Menu.OrderGoods','Menu.OrderGoods.Description','采购订货','~/Main.aspx?mid=Order.OrderGoods',1,null,getdate(),null,getdate(),null,null);
INSERT ACC_MenuCommon VALUES ('165','27',3,29,1,getdate(),null,getdate(),null);

update ACC_MenuIndustry set seq = 30 where menuid = 120;
--end dingxin


--begin tiansu 20100925 货运公司
alter table IpMstr add TransportCompany varchar(255);
GO
update IpMstr set TransportCompany='';

INSERT INTO "CodeMstr" (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('DeliverType','Express',10,1,'快递');
INSERT INTO "CodeMstr" (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('DeliverType','Other',20,0,'其它');
GO
--end tiansu 20100925


--begin tiansu 20100925  
	--去掉菜单	
	--	销售调整
	--	客户寄售
	--	拣货单提交/关闭 
	--  拣货单批次上线 
	--	样品发运
	--  样品采购单
	--  精益引擎 
	--  作业调度 
    --添加菜单
    --	销售管理->发货菜单
set IDENTITY_INSERT ACC_MenuCommon on;
INSERT INTO ACC_MenuCommon (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (67,'67','64',3,201,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null);
INSERT INTO ACC_MenuCommon (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (129,'129','64',3,387,1,'2010-07-15 10:20:15',null,'2010-07-15 10:20:15',null);
set IDENTITY_INSERT ACC_MenuCommon off;

update ACC_MenuCommon set isactive = 0 where id in (9,10,32,66,69,70,72,73);
GO
--end tiansu 20100925



--begin tiansu 20100925 加发货类型和快递号字段
alter table IpMstr add DeliverType varchar(50),ExpressNo varchar(50);
GO
update IpMstr set DeliverType='',ExpressNo='';
GO
--end tiansu 20100925


--begin tiansu 20100920 
INSERT INTO "CodeMstr" (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('OrderTemplate','PurchaseAbroad.xls',60,0,'销售单(海外)');
update "CodeMstr" set Desc1='合同' where code='OrderTemplate' and CodeValue='Contract.xls' and Seq=40 and IsDefault=0;
--end tiansu 20100920

--begin tiansu 20100920 
INSERT INTO "CodeMstr" (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('OrderTemplate','Contract.xls',40,0,'天熙合同');
INSERT INTO "CodeMstr" (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('OrderTemplate','Purchase.xls',50,0,'销售单');
alter table PartyAddr add Address2 varchar(255);
GO
update PartyAddr set Address2='';
GO
--end tiansu 20100920




--begin wangxiang 20100920 订单审批
INSERT INTO "ACC_Permission" (PM_Code,PM_Desc,PM_CateCode) VALUES ('ApproveOrder','订单同意','OrderOperation');
INSERT INTO "ACC_Permission" (PM_Code,PM_Desc,PM_CateCode) VALUES ('RejectOrder','订单否决','OrderOperation');
alter table ipmstr add ApprovalStatus varchar(50);
alter table ordermstr add ConOrderNo varchar(50);
INSERT INTO "EntityOpt" (PreCode,PreValue,CodeDesc,Seq) VALUES ('isShowDiscount','True','是否显示折扣',290)
alter table ordermstr add RelOrderNo varchar(50);
--end wangxiang 20100920


--begin tiansu 20100916 付款方式
INSERT INTO "CodeMstr" (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('PayType','PromissoryNote',10,1,'本票');
INSERT INTO "CodeMstr" (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('PayType','BankTransfer',20,0,'银行转帐');
INSERT INTO "CodeMstr" (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('PayType','Cash',30,0,'现金');
INSERT INTO "CodeMstr" (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('PayType','Cheque',40,0,'支票');
INSERT INTO "CodeMstr" (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('PayType','BankAcceptance',50,0,'银行承兑汇票');
INSERT INTO "CodeMstr" (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('PayType','TradeAcceptance',60,0,'商业承兑汇票');
INSERT INTO "CodeMstr" (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('PayType','DirectDebit',70,0,'贷记凭证');
INSERT INTO "CodeMstr" (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('PayType','CableTransfer',80,0,'电汇');
INSERT INTO "CodeMstr" (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('PayType','Hedging',90,0,'对冲');
GO
--end tiansu 20100916





--begin wangxiang 20100915
alter table flowdet add Brand varchar(50),Manufacturer varchar(50);
alter table orderdet add Brand varchar(50),Manufacturer varchar(50);
alter table ordermstr add ApprovalStatus varchar(50);

INSERT INTO "CodeMstr" (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('ApprovalStatus','Pending',10,0,'待审批');
INSERT INTO "CodeMstr" (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('ApprovalStatus','Rejected',20,0,'否决');
INSERT INTO "CodeMstr" (Code,CodeValue,Seq,IsDefault,Desc1) VALUES ('ApprovalStatus','Approved',10,0,'同意');

alter table flowmstr add Settlement varchar(max);
alter table ordermstr add Settlement varchar(max);

--end


--begin tiansu 20100914
INSERT acc_menu VALUES ('ItemCategory.166','Menu.ItemCategory.166',1,'Menu.ItemCategory','Menu.ItemCategory.Description','品名','~/Main.aspx?mid=MasterData.ItemCategory',1,'~/Images/Nav/ItemCategory.png',getdate(),null,getdate(),null,null);
GO
INSERT ACC_MenuCommon VALUES ('ItemCategory.166','11',2,27,1,getdate(),null,getdate(),null);
GO

if exists (select 1
            from  sysobjects
           where  id = object_id('ItemCategory')
            and   type = 'U')
   drop table ItemCategory
go

/*==============================================================*/
/* Table: ItemCategory                                          */
/*==============================================================*/
create table ItemCategory (
   Code                 varchar(50)          not null,
   Desc1                varchar(255)         null,
   Desc2                varchar(255)         null
)
go

alter table ItemCategory
   add constraint PK_ITEMCATEGORY primary key (Code)
go



alter table Item add Spec varchar(1000),Manufacturer varchar(50),Brand varchar(50),ItemCategoryCode varchar(50);
GO


alter table Item
   add constraint FK_ITEM_REFERENCE_ITEMCATE foreign key (ItemCategoryCode)
      references ItemCategory (Code)
go




insert into itemcategory(code,desc1,desc2) values('100','硬质合金刀片','');
insert into itemcategory(code,desc1,desc2) values('150','金刚石及CBN刀片','');
insert into itemcategory(code,desc1,desc2) values('200','钻头','');
insert into itemcategory(code,desc1,desc2) values('250','丝锥','');
insert into itemcategory(code,desc1,desc2) values('300','立铣刀','');
insert into itemcategory(code,desc1,desc2) values('350','铰刀','');
insert into itemcategory(code,desc1,desc2) values('400','车刀,镗刀,割槽刀,螺纹刀刀杆','');
insert into itemcategory(code,desc1,desc2) values('450','非标刀体(如mst产品)','');
insert into itemcategory(code,desc1,desc2) values('500','铣刀盘','');
insert into itemcategory(code,desc1,desc2) values('550','刀柄','');
insert into itemcategory(code,desc1,desc2) values('600','夹头','');
insert into itemcategory(code,desc1,desc2) values('650','刀夹','');
insert into itemcategory(code,desc1,desc2) values('700','复杂刀具（拉刀,滚刀）','');
insert into itemcategory(code,desc1,desc2) values('750','磨具/磨料等(如砂轮)','');
insert into itemcategory(code,desc1,desc2) values('800','量具类产品','');
insert into itemcategory(code,desc1,desc2) values('850','其他刀具或产品(上述代码中无归类的)','');
insert into itemcategory(code,desc1,desc2) values('900','配件','');
insert into itemcategory(code,desc1,desc2) values('950','修磨刀具','修磨刀具');
insert into itemcategory(code,desc1,desc2) values('980','棒料','棒料');
GO

--begin tiansu 20100914





--begin dx 20100914
alter table location add IsSetCS bit;
update location set IsSetCS = 0;
--end


--begin tiansu 20100914 
update BillMstr set [BackwashAmount] = 0 where [BackwashAmount] is null;


update BillMstr set [TotalBillDetailAmount] = 0 where [TotalBillDetailAmount] is null;

update BillMstr set [TotalBillAmount] = 0 where [TotalBillAmount] is null;

update BillMstr set [TotalBillDiscountRate] = 0 where [TotalBillDiscountRate] is null;
GO
--end tiansu 20100914



--begin liqiuyun 20100913 增加供应商-供应商寄售 菜单
INSERT acc_menu VALUES ('Menu.POPlanBill.165','Menu.POPlanBill.165',1,'Menu.POPlanBill','Menu.POPlanBill.Description','供应商寄售','~/Main.aspx?mid=Finance.PlanBill.PO__mp--ModuleType-PO_IsSupplier-true',1,'~/Images/Nav/POPlanBill.png','2010-07-15 10:15:12',null,'2010-07-15 10:15:12',null,null);
GO
--end liqiuyun 20100913






--begin tiansu 20100910 添加账单字段用于记录  已回冲金额 总金额 折扣金额 折扣率
ALTER  table   BillMstr
add	[BackwashAmount] [decimal](18, 8) NULL,
	[TotalBillDetailAmount] [decimal](18, 8) NULL,
	[TotalBillAmount] [decimal](18, 8) NULL,
	[TotalBillDiscountRate] [decimal](18, 8) NULL;
GO	
update BillMstr set [BackwashAmount]=0,[TotalBillDetailAmount]=0,[TotalBillAmount]=0,[TotalBillDiscountRate]=0;
GO
--end tansu 20100910 


--begin wangxiang 20100907 增加订单作废
set IDENTITY_INSERT "ACC_Permission" on;
INSERT INTO "ACC_Permission" (PM_ID,PM_Code,PM_Desc,PM_CateCode) VALUES (2870,'VoidOrder','订单作废','OrderOperation')
set IDENTITY_INSERT "ACC_Permission" off;
GO





--begin tiansu 20100908 付款权限

INSERT INTO "ACC_PermissionCategory" (PMC_Code,PMC_Desc,PMC_Type) VALUES ('PaymentOperation','付款操作','Page');

set IDENTITY_INSERT ACC_Permission on;
INSERT INTO "ACC_Permission" (PM_ID,PM_Code,PM_Desc,PM_CateCode)  VALUES( 2770,'EditPayment','编辑付款','PaymentOperation')
set IDENTITY_INSERT ACC_Permission off;
GO

--end tiansu 20100908


--begin tiansu 20100906 1)修改菜单 2) 加付款表,关联账单主表
update acc_menucommon set isactive=0 where menuid='156';
GO

INSERT INTO "ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('163','Menu.ProcurementPayment.163',1,'Menu.ProcurementPayment','Menu.ProcurementPayment.Description','采购付款','~/Main.aspx?mid=Finance.Payment__mp--ModuleType-PO',1,'~/Images/Nav/ProcurementPayment.png',getdate(),null,getdate(),null,'');
GO

INSERT INTO "ACC_Menu" (Id,Code,Version,Title,Description,Desc_,PageUrl,IsActive,ImageUrl,CreateDate,CreateUser,LastModifyDate,LastModifyUser,Remark) VALUES ('164','Menu.DistributionPayment.164',1,'Menu.DistributionPayment','Menu.DistributionPayment.Description','销售付款','~/Main.aspx?mid=Finance.Payment__mp--ModuleType-SO',1,'~/Images/Nav/DistributionPayment.png',getdate(),null,getdate(),null,'');
GO

set IDENTITY_INSERT ACC_MenuCommon on;

INSERT INTO "ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (163,'163','27',3,103,1,getdate(),null,getdate(),null);
GO

INSERT INTO "ACC_MenuCommon" (Id,MenuId,ParentMenuId,Level_,Seq,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES (164,'164','64',3,223,1,getdate(),null,getdate(),null);
GO

set IDENTITY_INSERT ACC_MenuCommon off;

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BillPayment') and o.name = 'FK_BILLPAYM_FK_BILL_P_PAYMENT')
alter table BillPayment
   drop constraint FK_BILLPAYM_FK_BILL_P_PAYMENT
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Payment') and o.name = 'FK_PAYMENT_FK_PAYMEN_PARTY')
alter table Payment
   drop constraint FK_PAYMENT_FK_PAYMEN_PARTY
go

alter table Payment
   drop constraint PK_PAYMENT
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Payment')
            and   type = 'U')
   drop table Payment
go

/*==============================================================*/
/* Table: Payment                                               */
/*==============================================================*/
create table Payment (
   PaymentNo            varchar(50)          not null,
   ExtPaymentNo         varchar(50)          null,
   PartyCode            varchar(50)          null,
   PaymentDate          datetime             null,
   RefPaymentNo         varchar(50)          null,
   InvoiceNo            varchar(50)          null,
   Amount               decimal(18,8)        not null,
   BackwashAmount       decimal(18,8)        null,
   Currency             varchar(50)          null,
   Status               varchar(50)          not null,
   TransType            varchar(50)          not null,
   PayType              varchar(50)          null,
   CreateDate           datetime             not null,
   CreateUser           varchar(50)          not null,
   LastModifyDate       datetime             not null,
   LastModifyUser       varchar(50)          not null
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '标志是否已经都回冲',
   'user', @CurrentUser, 'table', 'Payment', 'column', 'Status'
go

alter table Payment
   add constraint PK_PAYMENT primary key (PaymentNo)
go

alter table Payment
   add constraint FK_PAYMENT_FK_PAYMEN_PARTY foreign key (PartyCode)
      references Party (Code)
go














if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BillPayment') and o.name = 'FK_BILLPAYM_FK_BILL_P_BILLMSTR')
alter table BillPayment
   drop constraint FK_BILLPAYM_FK_BILL_P_BILLMSTR
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('BillPayment') and o.name = 'FK_BILLPAYM_FK_BILL_P_PAYMENT')
alter table BillPayment
   drop constraint FK_BILLPAYM_FK_BILL_P_PAYMENT
go

alter table BillPayment
   drop constraint PK_BILLPAYMENT
go

if exists (select 1
            from  sysobjects
           where  id = object_id('BillPayment')
            and   type = 'U')
   drop table BillPayment
go

/*==============================================================*/
/* Table: BillPayment                                           */
/*==============================================================*/
create table BillPayment (
   Id                   int                  identity(1, 1) not for replication,
   BillNo               varchar(50)          not null,
   PaymentNo            varchar(50)          not null,
   BackwashAmount       decimal(18,8)        null,
   CreateDate           datetime             not null,
   CreateUser           varchar(50)          not null,
   LastModifyDate       datetime             not null,
   LastModifyUser       varchar(50)          not null
)
go

alter table BillPayment
   add constraint PK_BILLPAYMENT primary key (Id)
go

alter table BillPayment
   add constraint FK_BILLPAYM_FK_BILL_P_BILLMSTR foreign key (BillNo)
      references BillMstr (BillNo)
go

alter table BillPayment
   add constraint FK_BILLPAYM_FK_BILL_P_PAYMENT foreign key (PaymentNo)
      references Payment (PaymentNo)
go







--end tiansu 20100906


--begin tiansu 20100902 修改菜单视图,禁用菜单

update acc_menucommon set isactive = 0 where menuid in ('21','135','136','137','138','139','140','141','143','144','147','148','22','23','24','25');

/****** 对象:  View [dbo].[MenuView]    脚本日期: 09/02/2010 09:52:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER VIEW [dbo].[MenuView]
AS
SELECT     Menu1.Id, Menu1.Code, Menu1.Version, Menu1.Title, Menu1.Desc_, Menu1.Description, Menu1.PageUrl, Menu1.IsActive, Menu1.ImageUrl, 
                      Menu1.Remark, dbo.ACC_MenuCommon.Id AS MenuRelationId, 'ACC_MenuCommon' AS Type, '' AS IndustryOrCompanyCode, 
                      dbo.ACC_MenuCommon.ParentMenuId AS ParentId, ParentMenu1.Code AS ParentCode, ParentMenu1.Version AS ParenVersion, 
                      dbo.ACC_MenuCommon.Level_, dbo.ACC_MenuCommon.Seq, dbo.ACC_MenuCommon.IsActive AS MenuRelationIsActive, 
                      dbo.ACC_MenuCommon.CreateDate, dbo.ACC_MenuCommon.CreateUser, dbo.ACC_MenuCommon.LastModifyDate, 
                      dbo.ACC_MenuCommon.LastModifyUser
FROM         dbo.ACC_MenuCommon INNER JOIN
                      dbo.ACC_Menu AS Menu1 ON Menu1.Id = dbo.ACC_MenuCommon.MenuId LEFT OUTER JOIN
                      dbo.ACC_Menu AS ParentMenu1 ON dbo.ACC_MenuCommon.ParentMenuId = ParentMenu1.Id
WHERE     (NOT EXISTS
                          (SELECT     1 AS Expr1
                            FROM          dbo.ACC_MenuIndustry INNER JOIN
                                                   dbo.ACC_Industry ON dbo.ACC_MenuIndustry.IndustryCode = dbo.ACC_Industry.Code INNER JOIN
                                                   dbo.ACC_Company ON dbo.ACC_Industry.Code = dbo.ACC_Company.IndustryCode INNER JOIN
                                                   dbo.EntityOpt ON dbo.ACC_Company.Code = dbo.EntityOpt.PreValue INNER JOIN
                                                   dbo.ACC_Menu ON dbo.ACC_MenuIndustry.MenuId = dbo.ACC_Menu.Id
                            WHERE      (dbo.EntityOpt.PreCode = 'CompanyCode') AND (dbo.ACC_Menu.Code = Menu1.Code))) AND (NOT EXISTS
                          (SELECT     1 AS Expr1
                            FROM          dbo.ACC_MenuCompany INNER JOIN
                                                   dbo.ACC_Company AS ACC_Company_3 ON dbo.ACC_MenuCompany.CompanyCode = ACC_Company_3.Code INNER JOIN
                                                   dbo.EntityOpt AS EntityOpt_3 ON ACC_Company_3.Code = EntityOpt_3.PreValue INNER JOIN
                                                   dbo.ACC_Menu AS ACC_Menu_2 ON dbo.ACC_MenuCompany.MenuId = ACC_Menu_2.Id
                            WHERE      (EntityOpt_3.PreCode = 'CompanyCode') AND (Menu1.Code = ACC_Menu_2.Code)))
UNION
SELECT     Menu2.Id, Menu2.Code, Menu2.Version, Menu2.Title, Menu2.Desc_, Menu2.Description, Menu2.PageUrl, Menu2.IsActive, Menu2.ImageUrl, 
                      Menu2.Remark, ACC_MenuIndustry_1.Id AS MenuRelationId, 'ACC_MenuIndustry' AS Type, 
                      ACC_MenuIndustry_1.IndustryCode AS IndustryOrCompanyCode, ACC_MenuIndustry_1.ParentMenuId AS ParentId, ParentMenu2.Code AS ParentCode, 
                      ParentMenu2.Version AS ParentVersion, ACC_MenuIndustry_1.Level_, ACC_MenuIndustry_1.Seq, 
                      ACC_MenuIndustry_1.IsActive AS MenuRelationIsActive, ACC_MenuIndustry_1.CreateDate, ACC_MenuIndustry_1.CreateUser, 
                      ACC_MenuIndustry_1.LastModifyDate, ACC_MenuIndustry_1.LastModifyUser
FROM         dbo.ACC_MenuIndustry AS ACC_MenuIndustry_1 INNER JOIN
                      dbo.ACC_Industry AS ACC_Industry_1 ON ACC_MenuIndustry_1.IndustryCode = ACC_Industry_1.Code INNER JOIN
                      dbo.ACC_Company AS ACC_Company_4 ON ACC_Company_4.IndustryCode = ACC_Industry_1.Code INNER JOIN
                      dbo.EntityOpt AS EntityOpt_4 ON ACC_Company_4.Code = EntityOpt_4.PreValue INNER JOIN
                      dbo.ACC_Menu AS Menu2 ON Menu2.Id = ACC_MenuIndustry_1.MenuId LEFT OUTER JOIN
                      dbo.ACC_Menu AS ParentMenu2 ON ACC_MenuIndustry_1.ParentMenuId = ParentMenu2.Id
WHERE     (EntityOpt_4.PreCode = 'CompanyCode') AND (NOT EXISTS
                          (SELECT     1 AS Expr1
                            FROM          dbo.ACC_MenuCompany AS ACC_MenuCompany_2 INNER JOIN
                                                   dbo.ACC_Company AS ACC_Company_2 ON ACC_MenuCompany_2.CompanyCode = ACC_Company_2.Code INNER JOIN
                                                   dbo.EntityOpt AS EntityOpt_2 ON ACC_Company_2.Code = EntityOpt_2.PreValue INNER JOIN
                                                   dbo.ACC_Menu AS ACC_Menu_1 ON ACC_MenuCompany_2.MenuId = ACC_Menu_1.Id
                            WHERE      (EntityOpt_2.PreCode = 'CompanyCode') AND (ACC_Menu_1.Code = Menu2.Code)))
UNION
SELECT     Menu3.Id, Menu3.Code, Menu3.Version, Menu3.Title, Menu3.Desc_, Menu3.Description, Menu3.PageUrl, Menu3.IsActive, Menu3.ImageUrl, 
                      Menu3.Remark, ACC_MenuCompany_1.Id AS MenuRelationId, 'ACC_MenuCompany' AS Type, 
                      ACC_MenuCompany_1.CompanyCode AS IndustryOrCompanyCode, ACC_MenuCompany_1.ParentMenuId AS ParentId, 
                      ParentMenu3.Code AS ParentCode, ParentMenu3.Version AS ParentVersion, ACC_MenuCompany_1.Level_, ACC_MenuCompany_1.Seq, 
                      ACC_MenuCompany_1.IsActive AS MenuRelationIsActive, ACC_MenuCompany_1.CreateDate, ACC_MenuCompany_1.CreateUser, 
                      ACC_MenuCompany_1.LastModifyDate, ACC_MenuCompany_1.LastModifyUser
FROM         dbo.ACC_MenuCompany AS ACC_MenuCompany_1 INNER JOIN
                      dbo.ACC_Menu AS Menu3 ON ACC_MenuCompany_1.MenuId = Menu3.Id INNER JOIN
                      dbo.ACC_Company AS ACC_Company_1 ON ACC_MenuCompany_1.CompanyCode = ACC_Company_1.Code INNER JOIN
                      dbo.EntityOpt AS EntityOpt_1 ON ACC_Company_1.Code = EntityOpt_1.PreValue LEFT OUTER JOIN
                      dbo.ACC_Menu AS ParentMenu3 ON ACC_MenuCompany_1.ParentMenuId = ParentMenu3.Id
WHERE     (EntityOpt_1.PreCode = 'CompanyCode')
GO
--end tiansu 20100902









-- begin 20100901 更新状态描述,中文显示 tiansu
update CodeMstr set desc1='取消' where codevalue='Cancel';
update CodeMstr set desc1='关闭' where codevalue='Close';
update CodeMstr set desc1='完成' where codevalue='Complete';
update CodeMstr set desc1='创建' where codevalue='Create';
update CodeMstr set desc1='执行中' where codevalue='In-Process';
update CodeMstr set desc1='提交' where codevalue='Submit';
update CodeMstr set desc1='作废' where codevalue='Void';
GO
-- end 20100901 tiansu

-- begin 20100831 [LocTransView] 加 LotNo 字段 tiansu
delete from ACC_MenuCommon where id in (8,15,18,19)

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER VIEW [dbo].[LocTransView]
AS
SELECT     MAX(Id) AS Id, OrderNo, ExtOrderNo, RefOrderNo, IpNo, RecNo, BillTransId, TransType, Item, ItemDesc, Uom, SUM(Qty) AS Qty, PartyFrom, 
                      PartyFromName, PartyTo, PartyToName, ShipFrom, ShipFromAddr, ShipTo, ShipToAddr, Loc, LocName, LocIOReason, LocIOReasonDesc, EffDate, 
                      CreateUser, LotNo
FROM         dbo.LocTrans
GROUP BY OrderNo, ExtOrderNo, RefOrderNo, IpNo, RecNo, BillTransId, TransType, Item, ItemDesc, Uom, PartyFrom, PartyFromName, PartyTo, PartyToName, 
                      ShipFrom, ShipFromAddr, ShipTo, ShipToAddr, Loc, LocName, LocIOReason, LocIOReasonDesc, EffDate, CreateUser, LotNo
GO
-- end 20100831 tiansu



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetNextSequence]
	@CodePrefix varchar(50),
	@NextSequence int OUTPUT
AS
Begin Tran
	Declare @invValue int;
	select  @invValue = IntValue FROM NumCtrl WITH (UPDLOCK, ROWLOCK) where Code = @CodePrefix;
	if @invValue is null
	begin
		if @NextSequence is not null
		begin 
			insert into NumCtrl(Code, IntValue) values (@CodePrefix, @NextSequence + 1);
		end	
		else
		begin
			set @NextSequence = 1;
			insert into NumCtrl(Code, IntValue) values (@CodePrefix, 2);
		end
	end 
	else
	begin
		if @NextSequence is not null
		begin 
			if @invValue <= @NextSequence
			begin
				update NumCtrl set IntValue = @NextSequence + 1 where Code = @CodePrefix;
			end
		end
		else
		begin
			set @NextSequence = @invValue;
			update NumCtrl set IntValue = @NextSequence + 1 where Code = @CodePrefix;
		end
	end	
Commit tran

-- begin 20100823 税率比例(%) tiansu
INSERT INTO "EntityOpt" (PreCode,PreValue,CodeDesc,Seq) VALUES ('TaxRate','17','税率比例(%)',71);
INSERT INTO "ACC_Industry" (Code,Title,Desc_,LogoUrl,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES ('QP','QP','汽车零配件行业',null,1,'2010-07-15 10:25:08',null,'2010-07-15 10:25:08',null);
INSERT INTO "ACC_Company" (Code,Title,Desc_,IndustryCode,LogoUrl,IsActive,CreateDate,CreateUser,LastModifyDate,LastModifyUser) VALUES ('ChunShen','ChunShen','春申','QP',null,1,'2010-07-16 09:43:59',null,'2010-07-16 09:43:59',null);
update "EntityOpt" set PreValue='TimesSeiko',CodeDesc='公司关键字' where PreCode='CompanyCode'
-- end 20100823 tiansu

ALTER table dbo.BillTrans
add	[BillDet] int NULL;

-- begin 20100820 记录是否含税 tiansu
ALTER  TABLE   PriceListMstr
		ADD	[TaxCode] [varchar](50) NULL,
			[IsIncludeTax] [bit] NULL;
		
UPDATE PriceListMstr SET IsIncludeTax = 0;
-- end 20100820 tiansu


-- begin 预留字段 tiansu
ALTER  table   dbo.OrderMstr
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[TextField5] [varchar](255) NULL,
	[TextField6] [varchar](255) NULL,
	[TextField7] [varchar](255) NULL,
	[TextField8] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[NumField5] [decimal](18, 8) NULL,
	[NumField6] [decimal](18, 8) NULL,
	[NumField7] [decimal](18, 8) NULL,
	[NumField8] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL,
	[DateField3] [datetime] NULL,
	[DateField4] [datetime] NULL;
	
	
ALTER  table   dbo.OrderDet
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[TextField5] [varchar](255) NULL,
	[TextField6] [varchar](255) NULL,
	[TextField7] [varchar](255) NULL,
	[TextField8] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[NumField5] [decimal](18, 8) NULL,
	[NumField6] [decimal](18, 8) NULL,
	[NumField7] [decimal](18, 8) NULL,
	[NumField8] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL,
	[DateField3] [datetime] NULL,
	[DateField4] [datetime] NULL;
	
	
ALTER  table   dbo.FlowMstr
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[TextField5] [varchar](255) NULL,
	[TextField6] [varchar](255) NULL,
	[TextField7] [varchar](255) NULL,
	[TextField8] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[NumField5] [decimal](18, 8) NULL,
	[NumField6] [decimal](18, 8) NULL,
	[NumField7] [decimal](18, 8) NULL,
	[NumField8] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL,
	[DateField3] [datetime] NULL,
	[DateField4] [datetime] NULL;
	
ALTER  table   dbo.FlowDet
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[TextField5] [varchar](255) NULL,
	[TextField6] [varchar](255) NULL,
	[TextField7] [varchar](255) NULL,
	[TextField8] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[NumField5] [decimal](18, 8) NULL,
	[NumField6] [decimal](18, 8) NULL,
	[NumField7] [decimal](18, 8) NULL,
	[NumField8] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL,
	[DateField3] [datetime] NULL,
	[DateField4] [datetime] NULL;
	
	

ALTER  table   dbo.BillMstr
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
	
ALTER  table   dbo.BillDet
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
	
ALTER  table   dbo.InspectMstr
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
	
ALTER  table   dbo.InspectDet
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
	
ALTER  table   dbo.IpMstr
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
ALTER  table   dbo.IpDet
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
ALTER  table   dbo.PickListMstr
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
ALTER  table   dbo.PickListDet
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
ALTER  table   dbo.PriceListMstr
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
ALTER  table   dbo.PriceListDet
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
ALTER  table   dbo.ReceiptMstr
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
ALTER  table   dbo.ReceiptDet
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
	
ALTER  table   dbo.Item
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
	
ALTER  table   dbo.Location
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
ALTER  table   dbo.Party
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
	
ALTER  table   dbo.PartyAddr
add	[TextField1] [varchar](255) NULL,
	[TextField2] [varchar](255) NULL,
	[TextField3] [varchar](255) NULL,
	[TextField4] [varchar](255) NULL,
	[NumField1] [decimal](18, 8) NULL,
	[NumField2] [decimal](18, 8) NULL,
	[NumField3] [decimal](18, 8) NULL,
	[NumField4] [decimal](18, 8) NULL,
	[DateField1] [datetime] NULL,
	[DateField2] [datetime] NULL;
go

-- end tiansu	


