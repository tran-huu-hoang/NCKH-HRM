using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NCKH_HRM.Models;

public partial class NckhDbContext : DbContext
{
    public NckhDbContext()
    {
    }

    public NckhDbContext(DbContextOptions<NckhDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<Class> Classes { get; set; }

    public virtual DbSet<CoursePoint> CoursePoints { get; set; }

    public virtual DbSet<DateLearn> DateLearns { get; set; }

    public virtual DbSet<DetailAtteandance> DetailAtteandances { get; set; }

    public virtual DbSet<DetailTerm> DetailTerms { get; set; }

    public virtual DbSet<Major> Majors { get; set; }

    public virtual DbSet<PointProcess> PointProcesses { get; set; }

    public virtual DbSet<PointSys4> PointSys4s { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<RegistStudent> RegistStudents { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<ScoreBoard> ScoreBoards { get; set; }

    public virtual DbSet<Semester> Semesters { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<StaffSubject> StaffSubjects { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<TeachingAssignment> TeachingAssignments { get; set; }

    public virtual DbSet<Term> Terms { get; set; }

    public virtual DbSet<Test> Tests { get; set; }

    public virtual DbSet<Timeline> Timelines { get; set; }

    public virtual DbSet<UserStaff> UserStaffs { get; set; }

    public virtual DbSet<UserStudent> UserStudents { get; set; }

    public virtual DbSet<Year> Years { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-2C5N86N9\\SQLEXPRESS; Database=NCKH_db; uid=sa; pwd=hoang1407; MultipleActiveResultSets=True; TrustServercertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ATTENDAN__3214EC271673E3EE");

            entity.ToTable("ATTENDANCE");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.DetailTerm).HasColumnName("DETAIL_TERM");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.RegistStudent).HasColumnName("REGIST_STUDENT");
            entity.Property(e => e.Status).HasColumnName("STATUS");
            entity.Property(e => e.Student).HasColumnName("STUDENT");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");

            entity.HasOne(d => d.DetailTermNavigation).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.DetailTerm)
                .HasConstraintName("FK__ATTENDANC__DETAI__04AFB25B");

            entity.HasOne(d => d.RegistStudentNavigation).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.RegistStudent)
                .HasConstraintName("FK__ATTENDANC__REGIS__05A3D694");

            entity.HasOne(d => d.StudentNavigation).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.Student)
                .HasConstraintName("FK__ATTENDANC__STUDE__03BB8E22");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CLASSES__3214EC27B1D4F396");

            entity.ToTable("CLASSES");

            entity.HasIndex(e => e.Code, "UQ__CLASSES__AA1D437908714145").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(25)
                .HasColumnName("CODE");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("NAME");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");
        });

        modelBuilder.Entity<CoursePoint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__COURSE_P__3214EC27C7185D96");

            entity.ToTable("COURSE_POINT");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.DetailTerm).HasColumnName("DETAIL_TERM");
            entity.Property(e => e.IdStaff).HasColumnName("ID_STAFF");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.NumberTest).HasColumnName("NUMBER_TEST");
            entity.Property(e => e.Overall4).HasColumnName("OVERALL_4");
            entity.Property(e => e.OverallScore).HasColumnName("OVERALL_SCORE");
            entity.Property(e => e.RegistStudent).HasColumnName("REGIST_STUDENT");
            entity.Property(e => e.ScoreBoard).HasColumnName("SCORE_BOARD");
            entity.Property(e => e.Status).HasColumnName("STATUS");
            entity.Property(e => e.Student).HasColumnName("STUDENT");
            entity.Property(e => e.TestScore).HasColumnName("TEST_SCORE");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");

            entity.HasOne(d => d.DetailTermNavigation).WithMany(p => p.CoursePoints)
                .HasForeignKey(d => d.DetailTerm)
                .HasConstraintName("FK__COURSE_PO__DETAI__0C50D423");

            entity.HasOne(d => d.IdStaffNavigation).WithMany(p => p.CoursePoints)
                .HasForeignKey(d => d.IdStaff)
                .HasConstraintName("FK__COURSE_PO__ID_ST__10216507");

            entity.HasOne(d => d.Overall4Navigation).WithMany(p => p.CoursePoints)
                .HasForeignKey(d => d.Overall4)
                .HasConstraintName("FK__COURSE_PO__OVERA__0F2D40CE");

            entity.HasOne(d => d.RegistStudentNavigation).WithMany(p => p.CoursePoints)
                .HasForeignKey(d => d.RegistStudent)
                .HasConstraintName("FK__COURSE_PO__REGIS__0D44F85C");

            entity.HasOne(d => d.ScoreBoardNavigation).WithMany(p => p.CoursePoints)
                .HasForeignKey(d => d.ScoreBoard)
                .HasConstraintName("FK__COURSE_PO__SCORE__0E391C95");

            entity.HasOne(d => d.StudentNavigation).WithMany(p => p.CoursePoints)
                .HasForeignKey(d => d.Student)
                .HasConstraintName("FK__COURSE_PO__STUDE__0B5CAFEA");
        });

        modelBuilder.Entity<DateLearn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DATE_LEA__3214EC271A62AFD6");

            entity.ToTable("DATE_LEARN");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.DetailTerm).HasColumnName("DETAIL_TERM");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.RegistStudent).HasColumnName("REGIST_STUDENT");
            entity.Property(e => e.Status).HasColumnName("STATUS");
            entity.Property(e => e.Student).HasColumnName("STUDENT");
            entity.Property(e => e.Timeline).HasColumnName("TIMELINE");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");

            entity.HasOne(d => d.DetailTermNavigation).WithMany(p => p.DateLearns)
                .HasForeignKey(d => d.DetailTerm)
                .HasConstraintName("FK__DATE_LEAR__DETAI__2057CCD0");

            entity.HasOne(d => d.RegistStudentNavigation).WithMany(p => p.DateLearns)
                .HasForeignKey(d => d.RegistStudent)
                .HasConstraintName("FK__DATE_LEAR__REGIS__214BF109");

            entity.HasOne(d => d.StudentNavigation).WithMany(p => p.DateLearns)
                .HasForeignKey(d => d.Student)
                .HasConstraintName("FK__DATE_LEAR__STUDE__1F63A897");

            entity.HasOne(d => d.TimelineNavigation).WithMany(p => p.DateLearns)
                .HasForeignKey(d => d.Timeline)
                .HasConstraintName("FK__DATE_LEAR__TIMEL__22401542");
        });

        modelBuilder.Entity<DetailAtteandance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DETAIL_A__3214EC2734EA7762");

            entity.ToTable("DETAIL_ATTEANDANCE");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CountAttend).HasColumnName("COUNT_ATTEND");
            entity.Property(e => e.CountLearn).HasColumnName("COUNT_LEARN");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.DetailTerm).HasColumnName("DETAIL_TERM");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.RegistStudent).HasColumnName("REGIST_STUDENT");
            entity.Property(e => e.Student).HasColumnName("STUDENT");
            entity.Property(e => e.Test1).HasColumnName("TEST1");
            entity.Property(e => e.Test2).HasColumnName("TEST2");
            entity.Property(e => e.Test3).HasColumnName("TEST3");
            entity.Property(e => e.Test4).HasColumnName("TEST4");
            entity.Property(e => e.Test5).HasColumnName("TEST5");
            entity.Property(e => e.Testavg).HasColumnName("TESTAVG");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");

            entity.HasOne(d => d.DetailTermNavigation).WithMany(p => p.DetailAtteandances)
                .HasForeignKey(d => d.DetailTerm)
                .HasConstraintName("FK__DETAIL_AT__DETAI__308E3499");

            entity.HasOne(d => d.RegistStudentNavigation).WithMany(p => p.DetailAtteandances)
                .HasForeignKey(d => d.RegistStudent)
                .HasConstraintName("FK__DETAIL_AT__REGIS__318258D2");

            entity.HasOne(d => d.StudentNavigation).WithMany(p => p.DetailAtteandances)
                .HasForeignKey(d => d.Student)
                .HasConstraintName("FK__DETAIL_AT__STUDE__2F9A1060");
        });

        modelBuilder.Entity<DetailTerm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DETAIL_T__3214EC27F01A69DC");

            entity.ToTable("DETAIL_TERM");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("END_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.MaxNumber).HasColumnName("MAX_NUMBER");
            entity.Property(e => e.Room)
                .HasMaxLength(255)
                .HasColumnName("ROOM");
            entity.Property(e => e.Semester).HasColumnName("SEMESTER");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("START_DATE");
            entity.Property(e => e.Term).HasColumnName("TERM");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");

            entity.HasOne(d => d.SemesterNavigation).WithMany(p => p.DetailTerms)
                .HasForeignKey(d => d.Semester)
                .HasConstraintName("FK__DETAIL_TE__SEMES__1BC821DD");

            entity.HasOne(d => d.TermNavigation).WithMany(p => p.DetailTerms)
                .HasForeignKey(d => d.Term)
                .HasConstraintName("FK__DETAIL_TER__TERM__1AD3FDA4");
        });

        modelBuilder.Entity<Major>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MAJOR__3214EC279F717215");

            entity.ToTable("MAJOR");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("NAME");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");
        });

        modelBuilder.Entity<PointProcess>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__POINT_PR__3214EC27978FB39A");

            entity.ToTable("POINT_PROCESS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Attendance).HasColumnName("ATTENDANCE");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.DetailTerm).HasColumnName("DETAIL_TERM");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.PointProcess1).HasColumnName("POINT_PROCESS");
            entity.Property(e => e.RegistStudent).HasColumnName("REGIST_STUDENT");
            entity.Property(e => e.ScoreBoard).HasColumnName("SCORE_BOARD");
            entity.Property(e => e.Status).HasColumnName("STATUS");
            entity.Property(e => e.Student).HasColumnName("STUDENT");
            entity.Property(e => e.Test).HasColumnName("TEST");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");

            entity.HasOne(d => d.AttendanceNavigation).WithMany(p => p.PointProcesses)
                .HasForeignKey(d => d.Attendance)
                .HasConstraintName("FK__POINT_PRO__ATTEN__55BFB948");

            entity.HasOne(d => d.DetailTermNavigation).WithMany(p => p.PointProcesses)
                .HasForeignKey(d => d.DetailTerm)
                .HasConstraintName("FK__POINT_PRO__DETAI__52E34C9D");

            entity.HasOne(d => d.RegistStudentNavigation).WithMany(p => p.PointProcesses)
                .HasForeignKey(d => d.RegistStudent)
                .HasConstraintName("FK__POINT_PRO__REGIS__53D770D6");

            entity.HasOne(d => d.ScoreBoardNavigation).WithMany(p => p.PointProcesses)
                .HasForeignKey(d => d.ScoreBoard)
                .HasConstraintName("FK__POINT_PRO__SCORE__54CB950F");

            entity.HasOne(d => d.StudentNavigation).WithMany(p => p.PointProcesses)
                .HasForeignKey(d => d.Student)
                .HasConstraintName("FK__POINT_PRO__STUDE__51EF2864");

            entity.HasOne(d => d.TestNavigation).WithMany(p => p.PointProcesses)
                .HasForeignKey(d => d.Test)
                .HasConstraintName("FK__POINT_PROC__TEST__56B3DD81");
        });

        modelBuilder.Entity<PointSys4>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__POINT_SY__3214EC27A1078950");

            entity.ToTable("POINT_SYS4");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Point).HasColumnName("POINT");
            entity.Property(e => e.Range1).HasColumnName("RANGE1");
            entity.Property(e => e.Range2).HasColumnName("RANGE2");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__POSITION__3214EC27475E34A7");

            entity.ToTable("POSITION");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("NAME");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");
        });

        modelBuilder.Entity<RegistStudent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__REGIST_S__3214EC27C2A8ABEA");

            entity.ToTable("REGIST_STUDENT");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.DetailTerm).HasColumnName("DETAIL_TERM");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Relearn).HasColumnName("RELEARN");
            entity.Property(e => e.Status).HasColumnName("STATUS");
            entity.Property(e => e.Student).HasColumnName("STUDENT");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");

            entity.HasOne(d => d.DetailTermNavigation).WithMany(p => p.RegistStudents)
                .HasForeignKey(d => d.DetailTerm)
                .HasConstraintName("FK__REGIST_ST__DETAI__7A3223E8");

            entity.HasOne(d => d.StudentNavigation).WithMany(p => p.RegistStudents)
                .HasForeignKey(d => d.Student)
                .HasConstraintName("FK__REGIST_ST__STUDE__793DFFAF");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ROLE__3214EC27C9298B33");

            entity.ToTable("ROLE");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("NAME");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");
        });

        modelBuilder.Entity<ScoreBoard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Score_Bo__3214EC2760551D2F");

            entity.ToTable("Score_Board");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Score).HasColumnName("SCORE");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");
        });

        modelBuilder.Entity<Semester>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SEMESTER__3214EC27268CBE4B");

            entity.ToTable("SEMESTER");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("END_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("NAME");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("START_DATE");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SESSION__3214EC2750ECBA68");

            entity.ToTable("SESSION");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("NAME");
            entity.Property(e => e.StartDate)
                .HasColumnType("date")
                .HasColumnName("START_DATE");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");
        });

        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__STAFF__3214EC27ADB16314");

            entity.ToTable("STAFF");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BirthDate)
                .HasColumnType("datetime")
                .HasColumnName("BIRTH_DATE");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.Degree)
                .HasMaxLength(255)
                .HasColumnName("DEGREE");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Gender).HasColumnName("GENDER");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Major).HasColumnName("MAJOR");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("NAME");
            entity.Property(e => e.NumberPhone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("NUMBER_PHONE");
            entity.Property(e => e.Position).HasColumnName("POSITION");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");
            entity.Property(e => e.Yearofwork).HasColumnName("YEAROFWORK");

            entity.HasOne(d => d.MajorNavigation).WithMany(p => p.Staff)
                .HasForeignKey(d => d.Major)
                .HasConstraintName("FK__STAFF__MAJOR__7C4F7684");

            entity.HasOne(d => d.PositionNavigation).WithMany(p => p.Staff)
                .HasForeignKey(d => d.Position)
                .HasConstraintName("FK__STAFF__POSITION__7D439ABD");
        });

        modelBuilder.Entity<StaffSubject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__STAFF_SU__3214EC2721FC8BF8");

            entity.ToTable("STAFF_SUBJECT");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Staff).HasColumnName("STAFF");
            entity.Property(e => e.Status).HasColumnName("STATUS");
            entity.Property(e => e.Subject).HasColumnName("SUBJECT");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");

            entity.HasOne(d => d.StaffNavigation).WithMany(p => p.StaffSubjects)
                .HasForeignKey(d => d.Staff)
                .HasConstraintName("FK__STAFF_SUB__STAFF__55F4C372");

            entity.HasOne(d => d.SubjectNavigation).WithMany(p => p.StaffSubjects)
                .HasForeignKey(d => d.Subject)
                .HasConstraintName("FK__STAFF_SUB__SUBJE__56E8E7AB");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__STUDENT__3214EC274072B8B6");

            entity.ToTable("STUDENT");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AccountNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ACCOUNT_NUMBER");
            entity.Property(e => e.Address)
                .HasMaxLength(2000)
                .HasColumnName("ADDRESS");
            entity.Property(e => e.BirthDate)
                .HasColumnType("date")
                .HasColumnName("BIRTH_DATE");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CITY");
            entity.Property(e => e.Classes).HasColumnName("CLASSES");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.CreateDateIdentityCard)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CREATE_DATE_IDENTITY_CARD");
            entity.Property(e => e.District)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DISTRICT");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Gender).HasColumnName("GENDER");
            entity.Property(e => e.IdentityCard)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("IDENTITY_CARD");
            entity.Property(e => e.Image)
                .HasMaxLength(2000)
                .HasColumnName("IMAGE");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Major).HasColumnName("MAJOR");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("NAME");
            entity.Property(e => e.NameBank)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAME_BANK");
            entity.Property(e => e.Nation)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NATION");
            entity.Property(e => e.Nationality)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NATIONALITY");
            entity.Property(e => e.Nationals)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NATIONALS");
            entity.Property(e => e.NumberPhone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("NUMBER_PHONE");
            entity.Property(e => e.PhoneFamily)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PHONE_FAMILY");
            entity.Property(e => e.PlaceIdentityCard)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PLACE_IDENTITY_CARD");
            entity.Property(e => e.Session).HasColumnName("SESSION");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("((1))")
                .HasColumnName("STATUS");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");
            entity.Property(e => e.Ward)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("WARD");

            entity.HasOne(d => d.ClassesNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.Classes)
                .HasConstraintName("FK__STUDENT__CLASSES__2BFE89A6");

            entity.HasOne(d => d.MajorNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.Major)
                .HasConstraintName("FK__STUDENT__MAJOR__2CF2ADDF");

            entity.HasOne(d => d.SessionNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.Session)
                .HasConstraintName("FK__STUDENT__SESSION__2B0A656D");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SUBJECT__3214EC27FC1A26C7");

            entity.ToTable("SUBJECT");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Major).HasColumnName("MAJOR");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("NAME");
            entity.Property(e => e.Status).HasColumnName("STATUS");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");

            entity.HasOne(d => d.MajorNavigation).WithMany(p => p.Subjects)
                .HasForeignKey(d => d.Major)
                .HasConstraintName("FK__SUBJECT__MAJOR__25518C17");
        });

        modelBuilder.Entity<TeachingAssignment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TEACHING__3214EC27141DC8FA");

            entity.ToTable("TEACHING_ASSIGNMENT");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.DetailTerm).HasColumnName("DETAIL_TERM");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Staff).HasColumnName("STAFF");
            entity.Property(e => e.Status).HasColumnName("STATUS");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");

            entity.HasOne(d => d.DetailTermNavigation).WithMany(p => p.TeachingAssignments)
                .HasForeignKey(d => d.DetailTerm)
                .HasConstraintName("FK__TEACHING___DETAI__4D5F7D71");

            entity.HasOne(d => d.StaffNavigation).WithMany(p => p.TeachingAssignments)
                .HasForeignKey(d => d.Staff)
                .HasConstraintName("FK__TEACHING___STAFF__4E53A1AA");
        });

        modelBuilder.Entity<Term>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TERM__3214EC275545E917");

            entity.ToTable("TERM");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CollegeCredit).HasColumnName("COLLEGE_CREDIT");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("NAME");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TEST__3214EC273D367F57");

            entity.ToTable("TEST");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DateLearn).HasColumnName("DATE_LEARN");
            entity.Property(e => e.DetailTerm).HasColumnName("DETAIL_TERM");
            entity.Property(e => e.IdAttendance).HasColumnName("ID_ATTENDANCE");
            entity.Property(e => e.Status).HasColumnName("STATUS");

            entity.HasOne(d => d.DateLearnNavigation).WithMany(p => p.Tests)
                .HasForeignKey(d => d.DateLearn)
                .HasConstraintName("FK__TEST__DATE_LEARN__39237A9A");

            entity.HasOne(d => d.DetailTermNavigation).WithMany(p => p.Tests)
                .HasForeignKey(d => d.DetailTerm)
                .HasConstraintName("FK__TEST__DETAIL_TER__382F5661");

            entity.HasOne(d => d.IdAttendanceNavigation).WithMany(p => p.Tests)
                .HasForeignKey(d => d.IdAttendance)
                .HasConstraintName("FK__TEST__ID_ATTENDA__373B3228");
        });

        modelBuilder.Entity<Timeline>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TIMELINE__3214EC2795ACE7A7");

            entity.ToTable("TIMELINE");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.DateLearn)
                .HasColumnType("datetime")
                .HasColumnName("DATE_LEARN");
            entity.Property(e => e.Isactive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("ISACTIVE");
            entity.Property(e => e.Isdelete).HasColumnName("ISDELETE");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("((1))")
                .HasColumnName("STATUS");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");
            entity.Property(e => e.Year).HasColumnName("YEAR");

            entity.HasOne(d => d.YearNavigation).WithMany(p => p.Timelines)
                .HasForeignKey(d => d.Year)
                .HasConstraintName("FK__TIMELINE__YEAR__339FAB6E");
        });

        modelBuilder.Entity<UserStaff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__USER_STA__3214EC27DAB58057");

            entity.ToTable("USER_STAFF");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy)
                .HasMaxLength(450)
                .HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete).HasColumnName("IS_DELETE");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Staff).HasColumnName("STAFF");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(450)
                .HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("UPDATE_DATE");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("USERNAME");

            entity.HasOne(d => d.StaffNavigation).WithMany(p => p.UserStaffs)
                .HasForeignKey(d => d.Staff)
                .HasConstraintName("FK__USER_STAF__STAFF__44CA3770");
        });

        modelBuilder.Entity<UserStudent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__USER_STU__3214EC27E1184090");

            entity.ToTable("USER_STUDENT");

            entity.HasIndex(e => e.Username, "UQ__USER_STU__B15BE12EF1BAC635").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CreateBy).HasColumnName("CREATE_BY");
            entity.Property(e => e.CreateDate)
                .HasMaxLength(450)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("CREATE_DATE");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("((1))")
                .HasColumnType("datetime")
                .HasColumnName("IS_ACTIVE");
            entity.Property(e => e.IsDelete)
                .HasColumnType("datetime")
                .HasColumnName("IS_DELETE");
            entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Student).HasColumnName("STUDENT");
            entity.Property(e => e.UpdateBy).HasColumnName("UPDATE_BY");
            entity.Property(e => e.UpdateDate)
                .HasMaxLength(450)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("UPDATE_DATE");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("USERNAME");

            entity.HasOne(d => d.StudentNavigation).WithMany(p => p.UserStudents)
                .HasForeignKey(d => d.Student)
                .HasConstraintName("FK__USER_STUD__STUDE__6FB49575");
        });

        modelBuilder.Entity<Year>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__YEAR__3214EC279E3C9C67");

            entity.ToTable("YEAR");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasColumnName("NAME");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
