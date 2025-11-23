using FreeExam.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FreeExam.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Question> Questions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // AppDbContext.cs - داخل protected override void OnModelCreating(ModelBuilder modelBuilder)

            // العلاقة 1: Question <--> Options (One-to-Many)
            // الـ Option هو الذي يحتوي على المفتاح الخارجي QuestionId
            modelBuilder.Entity<Option>()
                .HasOne(o => o.Question) // Option has one Question
                .WithMany(q => q.Options) // Question has many Options
                .HasForeignKey(o => o.QuestionId) // المفتاح الخارجي في Option
                .OnDelete(DeleteBehavior.Cascade);

            // العلاقة 2: Question <--> Answer (One-to-One)
            // الـ Answer هو الذي يمتلك المفتاح الخارجي QuestionId
            modelBuilder.Entity<Answer>()
                    .HasOne(a => a.Question)
                    .WithOne(q => q.Answer)
                    // هذا يؤكد أن QuestionId هو المفتاح الخارجي في كيان Answer
                    .HasForeignKey<Answer>(a => a.QuestionId);

            // العلاقة 3: Answer <--> Option (الإجابة الصحيحة)
            // الـ Answer هو الذي يمتلك المفتاح الخارجي OptionId
            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Option) // Answer has one Option
                .WithOne() // Option has one Answer (هذا قد يكون غير موجود في الـ Option Entity)
                .HasForeignKey<Answer>(a => a.OptionId) // المفتاح الخارجي في Answer
                .IsRequired(false); // قد تحتاج إلى هذا إذا كنت تريد السماح بحفظ إجابة مؤقتة بدون Option
        }

    }
}
