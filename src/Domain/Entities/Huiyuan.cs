using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WT.DirectLogistics.Domain.Entities
{
    public class Huiyuan
    {
        public int Id { get; set; }
        public string Cust_Name { get; set; }
        public string Truename { get; set; }
        public string Cust_Pass { get; set; }
        public string Pass_Note { get; set; }
        public string Pass_Answer { get; set; }
        public string Cust_Kind { get; set; }
        public string Yuangong { get; set; }
        public bool Vip { get; set; }
        public int Vipmid { get; set; }
        public bool Zhuangtai { get; set; }
        public DateTime? Time { get; set; }
        public double? Score { get; set; }
        public string Co { get; set; }
        public string Url { get; set; }
        public int? Num { get; set; }
        public string Zhuceren { get; set; }
        public string Zcrtel { get; set; }
        public int? Vnum { get; set; }
        public int? Vyear { get; set; }
        public bool? Vtype { get; set; }
        public int? Cishu { get; set; }
        public DateTime? Logintime { get; set; }
        public DateTime? Ipviptime { get; set; }
        public string Email { get; set; }
        public int? WapNum { get; set; }
        public int? SoftNum { get; set; }
        public DateTime? OpenVipTime { get; set; }
        public DateTime? CloseVipTime { get; set; }
        public int? Ispromoter { get; set; }
        public int? Verify { get; set; }
        public string Wxtnumber { get; set; }
        public string Domain { get; set; }
        public string Stylename { get; set; }
        public string Beiannum { get; set; }
        public int? Vlevel { get; set; }
        public double? ScoreGive { get; set; }
        public int? Recommend { get; set; }
        public string WebSite { get; set; }
        public int? Recommend1 { get; set; }
        public string WebSite1 { get; set; }
        public int? Price { get; set; }
        public int? GpsLoginNum { get; set; }
        public DateTime? OpenVipmidTime { get; set; }
        public DateTime? CloseVipmidTime { get; set; }
        public string OpenId { get; set; }
        public string WanshanRen { get; set; }
        public int? Issample { get; set; }
        public string CurrentPosition { get; set; }
        public int? ChengxinState { get; set; }
        public int? ScoreRemind { get; set; }
        public DateTime? Wanshan_Time { get; set; }
        public int? price_Add { get; set; }
        public int? ShimingState { get; set; }
        public int? RenzhengType { get; set; }
        public DateTime? PriceAddCloseTime { get; set; }
        public string Police_Beian { get; set; }
        public string Police_Beian_Txt { get; set; }
        public string Ph_Mac { get; set; }
        public DateTime? Ph_Bindtime { get; set; }
        public int? ShimingShenqingren { get; set; }
        public string InviteCode { get; set; }
        public int? IsCheckTel { get; set; }
        public string RenZhengTel { get; set; }
        public string register_ip { get; set; }
        public string VerifyInfo { get; set; }
        public string Unionid { get; set; }
        public int? Isopenwtms { get; set; }
        public int? Wx_Verify { get; set; }
        public string Kind_Note { get; set; }
        public double? Wx_GoldNum { get; set; }
        public int? Wx_Rmb { get; set; }
        public string Wx_Headpic { get; set; }
        public string Source_Beizhu { get; set; }
        public string Source_Url { get; set; }
        public DateTime? VerifyTime { get; set; }
        public DateTime? RenewVipTime { get; set; }
        public string RefreshToken { get; set; }
        public int? chengxin_score { get; set; }
        /// <summary>
        /// 登录来源
        /// </summary>
        [NotMapped]
        public string Source { get; set; }
        /// <summary>
        /// 是否校验redis key ： 1、不校验 2、所有端都登录、 3、每个端单点登录
        /// </summary>
        [NotMapped]
        public int LoginToken { get; set; }
    }
}
