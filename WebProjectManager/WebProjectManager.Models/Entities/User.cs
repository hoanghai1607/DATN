using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace WebProjectManager.Models.Entities
{
    public partial class User : IdentityUser<Guid>
    {
        public User()
        {
            AccountVerifications = new HashSet<AccountVerification>();
            CardUserMembers = new HashSet<CardUserMember>();
            Cards = new HashSet<Card>();
            Invites = new HashSet<Invite>();
            MemberProjects = new HashSet<MemberProject>();
            Projects = new HashSet<Project>();
            Tasks = new HashSet<Task>();
            TicketProjects = new HashSet<TicketProject>();
            Tickets = new HashSet<Ticket>();
            TimesheetProjects = new HashSet<TimesheetProject>();
            Timesheets = new HashSet<Timesheet>();
            Tabs = new HashSet<Tab>();
            TaskUserMember = new HashSet<TaskUserMember>();
        }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? UrlAvatar { get; set; }
        public string? Address { get; set; }
        public bool? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public bool? IsVerification { get; set; }
        public bool? IsActive { get; set; }
        public virtual ICollection<AccountVerification> AccountVerifications { get; set; }
        public virtual ICollection<CardUserMember> CardUserMembers { get; set; }
        public virtual ICollection<Card> Cards { get; set; }

        public virtual ICollection<Tab> Tabs { get; set; }
        public virtual ICollection<Invite> Invites { get; set; }
        public virtual ICollection<MemberProject> MemberProjects { get; set; }
        public virtual ICollection<Project> Projects { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<TicketProject> TicketProjects { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<TimesheetProject> TimesheetProjects { get; set; }
        public virtual ICollection<Timesheet> Timesheets { get; set; }
        public virtual ICollection<TaskUserMember> TaskUserMember { get; set; }
    }
}
