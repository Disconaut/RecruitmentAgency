using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using RecruitmentAgency.Models;

#nullable disable

namespace RecruitmentAgency.Data
{
    public partial class RecruiterAgencyContext : DbContext
    {
        public RecruiterAgencyContext()
        {
        }

        public RecruiterAgencyContext(DbContextOptions<RecruiterAgencyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<ApplicationStatus> ApplicationStatuses { get; set; }
        public virtual DbSet<Candidate> Candidates { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<CompanyOffice> CompanyOffices { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Interview> Interviews { get; set; }
        public virtual DbSet<JobCategory> JobCategories { get; set; }
        public virtual DbSet<JobTitle> JobTitles { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<PersonalAccount> PersonalAccounts { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SalaryTransaction> SalaryTransactions { get; set; }
        public virtual DbSet<Tariff> Tariffs { get; set; }
        public virtual DbSet<Vacancy> Vacancies { get; set; }
        public virtual DbSet<VmApplication> VmApplications { get; set; }
        public virtual DbSet<VmVacancy> VmVacancies { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Ukrainian_CI_AS");

            modelBuilder.Entity<Application>(entity =>
            {
                entity.ToTable("Application");

                entity.HasIndex(e => new { e.VacancyId, e.CandidateId }, "IX_Application")
                    .IsUnique();

                entity.HasOne(d => d.ApplicationStatus)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.ApplicationStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Application_ApplicationStatus");

                entity.HasOne(d => d.Candidate)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.CandidateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Application_Candidate");

                entity.HasOne(d => d.Vacancy)
                    .WithMany(p => p.Applications)
                    .HasForeignKey(d => d.VacancyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Application_Vacancy");
            });

            modelBuilder.Entity<ApplicationStatus>(entity =>
            {
                entity.ToTable("ApplicationStatus");

                entity.HasIndex(e => e.Name, "IX_ApplicationStatus")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<Candidate>(entity =>
            {
                entity.ToTable("Candidate");

                entity.HasIndex(e => e.Email, "IX_Candidate_UQ_Email")
                    .IsUnique()
                    .HasFilter("([Email] IS NOT NULL)");

                entity.HasIndex(e => e.PhoneNumber, "IX_Candidate_UQ_PhoneNumber")
                    .IsUnique()
                    .HasFilter("([PhoneNumber] IS NOT NULL)");

                entity.HasIndex(e => e.ResumeUrl, "IX_Candidate_UQ_ResumeUrl")
                    .IsUnique()
                    .HasFilter("([ResumeUrl] IS NOT NULL)");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Rating).HasColumnType("decimal(3, 2)");

                entity.Property(e => e.ResumeUrl)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Candidates)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Candidate_Department");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("City");

                entity.HasIndex(e => e.Name, "IX_City")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<CompanyOffice>(entity =>
            {
                entity.ToTable("CompanyOffice");

                entity.HasIndex(e => new { e.CityId, e.Address }, "IX_CompanyCity")
                    .IsUnique();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.CompanyOffices)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CompanyOf__CityI__51300E55");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.CompanyOffices)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__CompanyOf__Compa__5224328E");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.HasIndex(e => e.Email, "IX_Customer_UQ_Email")
                    .IsUnique()
                    .HasFilter("([Email] IS NOT NULL)");

                entity.HasIndex(e => e.PhoneNumber, "IX_Customer_UQ_PhoneNumber")
                    .IsUnique()
                    .HasFilter("([PhoneNumber] IS NOT NULL)");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Rating).HasColumnType("decimal(3, 2)");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer_Company");
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.ToTable("Department");

                entity.HasIndex(e => new { e.CityId, e.Address }, "IX_Department")
                    .IsUnique();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Departments)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Department_City");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Rating).HasColumnType("decimal(3, 2)");

                entity.HasOne(d => d.Department)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.DepartmentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee_Department");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Employee_Role");
            });

            modelBuilder.Entity<Interview>(entity =>
            {
                entity.ToTable("Interview");

                entity.Property(e => e.Feedback).HasMaxLength(1000);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.Interviews)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Interview_Application");
            });

            modelBuilder.Entity<JobCategory>(entity =>
            {
                entity.ToTable("JobCategory");

                entity.HasIndex(e => e.Name, "IX_JobCategory")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<JobTitle>(entity =>
            {
                entity.ToTable("JobTitle");

                entity.HasIndex(e => new { e.JobCategoryId, e.Name }, "IX_JobTitle")
                    .IsUnique();

                entity.Property(e => e.Description).HasColumnType("text");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.JobCategory)
                    .WithMany(p => p.JobTitles)
                    .HasForeignKey(d => d.JobCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_JobTitle_JobCategory");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.Sum).HasColumnType("money");

                entity.Property(e => e.TransactionDate).HasColumnType("date");

                entity.HasOne(d => d.Vacancy)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.VacancyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Payment_Vacancy");
            });

            modelBuilder.Entity<PersonalAccount>(entity =>
            {
                entity.ToTable("PersonalAccount");

                entity.HasIndex(e => e.EmployeeId, "IX_PersonalAccount")
                    .IsUnique();

                entity.HasIndex(e => e.Login, "IX_PersonalAccount_1")
                    .IsUnique();

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.Employee)
                    .WithOne(p => p.PersonalAccount)
                    .HasForeignKey<PersonalAccount>(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PersonalAccount_Employee");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.HasIndex(e => e.Name, "IX_Role")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PercentForVacancy).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.Salary).HasColumnType("money");
            });

            modelBuilder.Entity<SalaryTransaction>(entity =>
            {
                entity.ToTable("SalaryTransaction");

                entity.Property(e => e.Sum).HasColumnType("money");

                entity.Property(e => e.TransactionNum)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.SalaryTransactions)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalaryTransaction_Employee");
            });

            modelBuilder.Entity<Tariff>(entity =>
            {
                entity.ToTable("Tariff");

                entity.Property(e => e.BeginDate).HasColumnType("date");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.PriceForCandidate).HasColumnType("money");

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.Tariffs)
                    .HasForeignKey(d => d.ManagerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Tariff_Employee");
            });

            modelBuilder.Entity<Vacancy>(entity =>
            {
                entity.ToTable("Vacancy");

                entity.Property(e => e.CompanyOfficeId).HasDefaultValueSql("((1))");

                entity.Property(e => e.Requirements)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.HasOne(d => d.CompanyOffice)
                    .WithMany(p => p.Vacancies)
                    .HasForeignKey(d => d.CompanyOfficeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Vacancy__Company__07C12930");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Vacancies)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vacancy_Customer");

                entity.HasOne(d => d.JobTitle)
                    .WithMany(p => p.Vacancies)
                    .HasForeignKey(d => d.JobTitleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vacancy_JobTitle");

                entity.HasOne(d => d.Recruiter)
                    .WithMany(p => p.Vacancies)
                    .HasForeignKey(d => d.RecruiterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vacancy_Employee");

                entity.HasOne(d => d.Tariff)
                    .WithMany(p => p.Vacancies)
                    .HasForeignKey(d => d.TariffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vacancy_Tariff");
            });

            modelBuilder.Entity<VmApplication>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VM_Application");

                entity.Property(e => e.Candidate)
                    .IsRequired()
                    .HasMaxLength(61);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Customer)
                    .IsRequired()
                    .HasMaxLength(61);

                entity.Property(e => e.CustomerCompany)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.JobCategory)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.JobTitle)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Recruiter)
                    .IsRequired()
                    .HasMaxLength(61);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<VmVacancy>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VM_Vacancy");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Customer)
                    .IsRequired()
                    .HasMaxLength(61);

                entity.Property(e => e.CustomerCompany)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.JobCategory)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.JobTitle)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.Recruiter)
                    .IsRequired()
                    .HasMaxLength(61);

                entity.Property(e => e.Tariff).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
