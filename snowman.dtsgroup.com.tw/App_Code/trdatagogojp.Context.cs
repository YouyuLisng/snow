﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

public partial class trdatagogojpEntities : DbContext
{
    public trdatagogojpEntities()
        : base("name=trdatagogojpEntities")
    {
    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }

    public virtual DbSet<TRMGBOOK> TRMGBOOK { get; set; }
    public virtual DbSet<TRMGHTL> TRMGHTL { get; set; }
    public virtual DbSet<TRMGITN> TRMGITN { get; set; }
    public virtual DbSet<TRMGLOCL> TRMGLOCL { get; set; }
    public virtual DbSet<TRMGRUP> TRMGRUP { get; set; }
    public virtual DbSet<TRMGVISA> TRMGVISA { get; set; }
    public virtual DbSet<TRGRUP> TRGRUP { get; set; }
    public virtual DbSet<TRDGCLN> TRDGCLN { get; set; }
    public virtual DbSet<TRDGHTL> TRDGHTL { get; set; }
    public virtual DbSet<TRDGINC> TRDGINC { get; set; }
    public virtual DbSet<TRDGINS> TRDGINS { get; set; }
    public virtual DbSet<TRDGITN> TRDGITN { get; set; }
    public virtual DbSet<TRDGPAX> TRDGPAX { get; set; }
    public virtual DbSet<TRDGRES> TRDGRES { get; set; }
    public virtual DbSet<TRDGSC> TRDGSC { get; set; }
    public virtual DbSet<TRDGTRF> TRDGTRF { get; set; }
    public virtual DbSet<TRDGVS> TRDGVS { get; set; }
    public virtual DbSet<TRDPCRO> TRDPCRO { get; set; }
    public virtual DbSet<TRDPRC> TRDPRC { get; set; }
    public virtual DbSet<TRGBOOK> TRGBOOK { get; set; }
    public virtual DbSet<TRGHTL> TRGHTL { get; set; }
    public virtual DbSet<TRGITN> TRGITN { get; set; }
    public virtual DbSet<TRGLOCL> TRGLOCL { get; set; }
    public virtual DbSet<TRGPAX> TRGPAX { get; set; }
    public virtual DbSet<TRGVISA> TRGVISA { get; set; }
    public virtual DbSet<TRREC> TRREC { get; set; }
    public virtual DbSet<TRRECP> TRRECP { get; set; }
    public virtual DbSet<TRTCOST> TRTCOST { get; set; }
    public virtual DbSet<TRTORDS> TRTORDS { get; set; }
    public virtual DbSet<TRTRANS> TRTRANS { get; set; }
    public virtual DbSet<TRTREC> TRTREC { get; set; }
    public virtual DbSet<TRSUBD> TRSUBD { get; set; }
    public virtual DbSet<TRWORD> TRWORD { get; set; }
    public virtual DbSet<SE_TNAREA_GO> SE_TNAREA_GO { get; set; }
    public virtual DbSet<SE_TNGRUP_GO> SE_TNGRUP_GO { get; set; }
    public virtual DbSet<SE_TNSUBD_GO> SE_TNSUBD_GO { get; set; }
    public virtual DbSet<SEL_TNGRUP_GO> SEL_TNGRUP_GO { get; set; }
    public virtual DbSet<TRAIRP> TRAIRP { get; set; }
    public virtual DbSet<TRAIRP_DC> TRAIRP_DC { get; set; }
    public virtual DbSet<TRCAREA> TRCAREA { get; set; }
    public virtual DbSet<TRCARR> TRCARR { get; set; }
    public virtual DbSet<TRPORT> TRPORT { get; set; }
    public virtual DbSet<TRPORTDEP_TW> TRPORTDEP_TW { get; set; }
    public virtual DbSet<TRCITY> TRCITY { get; set; }
    public virtual DbSet<TRGSALEDTL> TRGSALEDTL { get; set; }
    public virtual DbSet<TRGOPITEM> TRGOPITEM { get; set; }
    public virtual DbSet<TRMPGITN> TRMPGITN { get; set; }
    public virtual DbSet<TRGRUPD1> TRGRUPD1 { get; set; }
    public virtual DbSet<TRGRUPSUBD> TRGRUPSUBD { get; set; }
    public virtual DbSet<TRACNTH> TRACNTH { get; set; }
    public virtual DbSet<TRAORD> TRAORD { get; set; }
    public virtual DbSet<TRCREC> TRCREC { get; set; }
    public virtual DbSet<TRDBATV> TRDBATV { get; set; }
    public virtual DbSet<TRENTRY> TRENTRY { get; set; }
    public virtual DbSet<TRGPAXLOG> TRGPAXLOG { get; set; }
    public virtual DbSet<TRGRUPFEAT> TRGRUPFEAT { get; set; }
    public virtual DbSet<TRGSALE> TRGSALE { get; set; }
    public virtual DbSet<TRKCREC> TRKCREC { get; set; }
    public virtual DbSet<TRKORD> TRKORD { get; set; }
    public virtual DbSet<TRPAX> TRPAX { get; set; }
    public virtual DbSet<TRPAXLOG> TRPAXLOG { get; set; }
    public virtual DbSet<TRRCVD> TRRCVD { get; set; }
    public virtual DbSet<TRRECLOG> TRRECLOG { get; set; }
    public virtual DbSet<TRTHST> TRTHST { get; set; }
    public virtual DbSet<TRTORD> TRTORD { get; set; }
    public virtual DbSet<TRTORDD> TRTORDD { get; set; }
    public virtual DbSet<TRTRANSD2> TRTRANSD2 { get; set; }
    public virtual DbSet<TRPGSUBD> TRPGSUBD { get; set; }
    public virtual DbSet<TRDBATD> TRDBATD { get; set; }
}
