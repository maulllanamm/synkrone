using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using synkrone.Model.Entities;

namespace synkrone.Model.Configuration;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> entity)
    {
        entity.ToTable("messages");

        // Primary Key
        entity.HasKey(m => m.Id);

        // Required field
        entity.Property(m => m.Content)
            .IsRequired();

        // Optional fields
        entity.Property(m => m.AttachmentUrl)
            .HasMaxLength(500); 

        // Enum type
        entity.Property(m => m.Type)
            .HasConversion<string>() 
            .IsRequired();

        // Timestamp defaults
        entity.Property(m => m.SentAt)
            .HasDefaultValueSql("NOW()");

        entity.Property(m => m.IsEdited)
            .HasDefaultValue(false);

        entity.Property(m => m.IsDeleted)
            .HasDefaultValue(false);

        // Relationships

        // Sender (required)
        entity.HasOne(m => m.Sender)
            .WithMany(u => u.SentMessages)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Receiver (optional - for private message)
        entity.HasOne(m => m.Receiver)
            .WithMany()
            .HasForeignKey(m => m.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        // Group (optional - for group message)
        entity.HasOne(m => m.Group)
            .WithMany(g => g.Messages)
            .HasForeignKey(m => m.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // Reply to another message (optional)
        entity.HasOne(m => m.ReplyToMessage)
            .WithMany()
            .HasForeignKey(m => m.ReplyToMessageId)
            .OnDelete(DeleteBehavior.Restrict);

        // Check constraint: Message must be either private OR group
        entity.HasCheckConstraint("CK_Message_Type",
            "(\"ReceiverId\" IS NOT NULL AND \"GroupId\" IS NULL) OR (\"ReceiverId\" IS NULL AND \"GroupId\" IS NOT NULL)");
    }
}
