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

    public virtual DbSet<DateLearn> DateLearns { get; set; }

    public virtual DbSet<DetailAttendance> DetailAttendances { get; set; }

    public virtual DbSet<DetailTerm> DetailTerms { get; set; }

    public virtual DbSet<Major> Majors { get; set; }

    public virtual DbSet<PointProcess> PointProcesses { get; set; }

    public virtual DbSet<Position> Positions { get; set; }

    public virtual DbSet<RegistStudent> RegistStudents { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Semester> Semesters { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<Staff> Staff { get; set; }

    public virtual DbSet<StaffSubject> StaffSubjects { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<TeachingAssignment> TeachingAssignments { get; set; }

    public virtual DbSet<Term> Terms { get; set; }

    public virtual DbSet<Timeline> Timelines { get; set; }

    public virtual DbSet<UserStaff> UserStaffs { get; set; }

    public virtual DbSet<UserStudent> UserStudents { get; set; }

    public virtual DbSet<Year> Years { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS; Database=NCKH_db; uid=sa; pwd=hoang1407; MultipleActiveResultSets=True;TrustServercertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ATTENDAN__3214EC276B6A52C4");

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
                .HasConstraintName("FK__ATTENDANC__DETAI__58D1301D");

            entity.HasOne(d => d.RegistStudentNavigation).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.RegistStudent)
                .HasConstraintName("FK__ATTENDANC__REGIS__59C55456");

            entity.HasOne(d => d.StudentNavigation).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.Student)
                .HasConstraintName("FK__ATTENDANC__STUDE__57DD0BE4");
        });

        modelBuilder.Entity<Class>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CLASSES__3214EC27BE81BBB6");

            entity.ToTable("CLASSES");

            entity.HasIndex(e => e.Code, "UQ__CLASSES__AA1D4379F303897F").IsUnique();

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

        modelBuilder.Entity<DateLearn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DATE_LEA__3214EC27FBC13D07");

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
                .HasConstraintName("FK__DATE_LEAR__DETAI__681373AD");

            entity.HasOne(d => d.RegistStudentNavigation).WithMany(p => p.DateLearns)
                .HasForeignKey(d => d.RegistStudent)
                .HasConstraintName("FK__DATE_LEAR__REGIS__690797E6");

            entity.HasOne(d => d.StudentNavigation).WithMany(p => p.DateLearns)
                .HasForeignKey(d => d.Student)
                .HasConstraintName("FK__DATE_LEAR__STUDE__671F4F74");

            entity.HasOne(d => d.TimelineNavigation).WithMany(p => p.DateLearns)
                .HasForeignKey(d => d.Timeline)
                .HasConstraintName("FK__DATE_LEAR__TIMEL__69FBBC1F");
        });

        modelBuilder.Entity<DetailAttendance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DETAIL_A__3214EC274995C5C8");

            entity.ToTable("DETAIL_ATTENDANCE");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DateLearn).HasColumnName("DATE_LEARN");
            entity.Property(e => e.DetailTerm).HasColumnName("DETAIL_TERM");
            entity.Property(e => e.IdAttendance).HasColumnName("ID_ATTENDANCE");
            entity.Property(e => e.Status).HasColumnName("STATUS");

            entity.HasOne(d => d.DateLearnNavigation).WithMany(p => p.DetailAttendances)
                .HasForeignKey(d => d.DateLearn)
                .HasConstraintName("FK__DETAIL_AT__DATE___7B264821");

            entity.HasOne(d => d.DetailTermNavigation).WithMany(p => p.DetailAttendances)
                .HasForeignKey(d => d.DetailTerm)
                .HasConstraintName("FK__DETAIL_AT__DETAI__7A3223E8");

            entity.HasOne(d => d.IdAttendanceNavigation).WithMany(p => p.DetailAttendances)
                .HasForeignKey(d => d.IdAttendance)
                .HasConstraintName("FK__DETAIL_AT__ID_AT__793DFFAF");
        });

        modelBuilder.Entity<DetailTerm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DETAIL_T__3214EC27A35FFC62");

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
                .HasConstraintName("FK__DETAIL_TE__SEMES__34C8D9D1");

            entity.HasOne(d => d.TermNavigation).WithMany(p => p.DetailTerms)
                .HasForeignKey(d => d.Term)
                .HasConstraintName("FK__DETAIL_TER__TERM__33D4B598");
        });

        modelBuilder.Entity<Major>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MAJOR__3214EC27C9C78A29");

            entity.ToTable("MAJOR");

            entity.HasIndex(e => e.Code, "UQ__MAJOR__AA1D4379B4112C10").IsUnique();

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

        modelBuilder.Entity<PointProcess>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__POINT_PR__3214EC271F588DC2");

            entity.ToTable("POINT_PROCESS");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Attendance).HasColumnName("ATTENDANCE");
            entity.Property(e => e.ComponentPoint).HasColumnName("COMPONENT_POINT");
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
            entity.Property(e => e.MidtermPoint).HasColumnName("MIDTERM_POINT");
            entity.Property(e => e.NumberTest).HasColumnName("NUMBER_TEST");
            entity.Property(e => e.OverallScore).HasColumnName("OVERALL_SCORE");
            entity.Property(e => e.RegistStudent).HasColumnName("REGIST_STUDENT");
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

            entity.HasOne(d => d.AttendanceNavigation).WithMany(p => p.PointProcesses)
                .HasForeignKey(d => d.Attendance)
                .HasConstraintName("FK__POINT_PRO__ATTEN__72910220");

            entity.HasOne(d => d.DetailTermNavigation).WithMany(p => p.PointProcesses)
                .HasForeignKey(d => d.DetailTerm)
                .HasConstraintName("FK__POINT_PRO__DETAI__70A8B9AE");

            entity.HasOne(d => d.IdStaffNavigation).WithMany(p => p.PointProcesses)
                .HasForeignKey(d => d.IdStaff)
                .HasConstraintName("FK__POINT_PRO__ID_ST__73852659");

            entity.HasOne(d => d.RegistStudentNavigation).WithMany(p => p.PointProcesses)
                .HasForeignKey(d => d.RegistStudent)
                .HasConstraintName("FK__POINT_PRO__REGIS__719CDDE7");

            entity.HasOne(d => d.StudentNavigation).WithMany(p => p.PointProcesses)
                .HasForeignKey(d => d.Student)
                .HasConstraintName("FK__POINT_PRO__STUDE__6FB49575");
        });

        modelBuilder.Entity<Position>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__POSITION__3214EC27CF859096");

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
            entity.HasKey(e => e.Id).HasName("PK__REGIST_S__3214EC27C959C759");

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
                .HasConstraintName("FK__REGIST_ST__DETAI__4E53A1AA");

            entity.HasOne(d => d.StudentNavigation).WithMany(p => p.RegistStudents)
                .HasForeignKey(d => d.Student)
                .HasConstraintName("FK__REGIST_ST__STUDE__4D5F7D71");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ROLE__3214EC27C6CEA480");

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

        modelBuilder.Entity<Semester>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SEMESTER__3214EC2754D3107A");

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
            entity.HasKey(e => e.Id).HasName("PK__SESSION__3214EC27536780CE");

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
            entity.HasKey(e => e.Id).HasName("PK__STAFF__3214EC27726ADF0A");

            entity.ToTable("STAFF");

            entity.HasIndex(e => e.Code, "UQ__STAFF__AA1D4379F407C481").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BirthDate)
                .HasColumnType("datetime")
                .HasColumnName("BIRTH_DATE");
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
                .HasConstraintName("FK__STAFF__MAJOR__656C112C");

            entity.HasOne(d => d.PositionNavigation).WithMany(p => p.Staff)
                .HasForeignKey(d => d.Position)
                .HasConstraintName("FK__STAFF__POSITION__66603565");
        });

        modelBuilder.Entity<StaffSubject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__STAFF_SU__3214EC27309A62D8");

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
                .HasConstraintName("FK__STAFF_SUB__STAFF__2FCF1A8A");

            entity.HasOne(d => d.SubjectNavigation).WithMany(p => p.StaffSubjects)
                .HasForeignKey(d => d.Subject)
                .HasConstraintName("FK__STAFF_SUB__SUBJE__30C33EC3");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__STUDENT__3214EC2752AEDFE5");

            entity.ToTable("STUDENT");

            entity.HasIndex(e => e.Code, "UQ__STUDENT__AA1D43799B272382").IsUnique();

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
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("CODE");
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
                .HasConstraintName("FK__STUDENT__CLASSES__0C85DE4D");

            entity.HasOne(d => d.MajorNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.Major)
                .HasConstraintName("FK__STUDENT__MAJOR__0D7A0286");

            entity.HasOne(d => d.SessionNavigation).WithMany(p => p.Students)
                .HasForeignKey(d => d.Session)
                .HasConstraintName("FK__STUDENT__SESSION__0B91BA14");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SUBJECT__3214EC27A0182625");

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
                .HasConstraintName("FK__SUBJECT__MAJOR__04E4BC85");
        });

        modelBuilder.Entity<TeachingAssignment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TEACHING__3214EC27CB3F92C1");

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
                .HasConstraintName("FK__TEACHING___DETAI__2739D489");

            entity.HasOne(d => d.StaffNavigation).WithMany(p => p.TeachingAssignments)
                .HasForeignKey(d => d.Staff)
                .HasConstraintName("FK__TEACHING___STAFF__282DF8C2");
        });

        modelBuilder.Entity<Term>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TERM__3214EC27FC2F0E04");

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

        modelBuilder.Entity<Timeline>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TIMELINE__3214EC2773AEAE94");

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
                .HasConstraintName("FK__TIMELINE__YEAR__14270015");
        });

        modelBuilder.Entity<UserStaff>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__USER_STA__3214EC27A67541A6");

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
                .HasConstraintName("FK__USER_STAF__STAFF__1F98B2C1");
        });

        modelBuilder.Entity<UserStudent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__USER_STU__3214EC27CB86E8DB");

            entity.ToTable("USER_STUDENT");

            entity.HasIndex(e => e.Username, "UQ__USER_STU__B15BE12E9EE5999C").IsUnique();

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
            entity.Property(e => e.Student).HasColumnName("STUDENT");
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

            entity.HasOne(d => d.StudentNavigation).WithMany(p => p.UserStudents)
                .HasForeignKey(d => d.Student)
                .HasConstraintName("FK__USER_STUD__STUDE__44CA3770");
        });

        modelBuilder.Entity<Year>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__YEAR__3214EC2776941210");

            entity.ToTable("YEAR");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasColumnName("NAME");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
