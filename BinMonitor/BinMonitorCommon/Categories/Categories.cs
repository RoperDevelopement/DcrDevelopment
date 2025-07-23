using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinMonitor.Common
{
    public sealed class SpecimenCategories : SerializedObjectDictionary<Category>
    {
        public override string DirectoryPath
        { get { return @"Config\Categories"; } }

        static readonly SpecimenCategories _Instance = new SpecimenCategories();
        public static SpecimenCategories Instance
        { get { return _Instance; } }

        static SpecimenCategories()
        {
            string emailRecipients = "sam.brinly@edocsusa.com";
            return;
            /*
            Category routine = new Category() { Color = new SerializableColor(Color.Brown), Title = ROUTINE_TITLE, };
            routine.CheckPoint1Configuration = new CheckPointConfiguration() { Duration = TimeSpan.FromMinutes(15), EmailImmediately = false, EmailUntilComplete = false, EmailFrequency = TimeSpan.MaxValue, Flash = false, EmailRecipients = emailRecipients };
            routine.CheckPoint2Configuration = new CheckPointConfiguration() { Duration = TimeSpan.FromMinutes(30), EmailImmediately = false, EmailUntilComplete = false, EmailFrequency = TimeSpan.MaxValue, Flash = false, EmailRecipients = emailRecipients };
            routine.CheckPoint3Configuration = new CheckPointConfiguration() { Duration = TimeSpan.FromMinutes(45), EmailImmediately = false, EmailUntilComplete = false, EmailFrequency = TimeSpan.MaxValue, Flash = false, EmailRecipients = emailRecipients };
            routine.CheckPoint4Configuration = new CheckPointConfiguration() { Duration = TimeSpan.FromMinutes(60), EmailImmediately = true, EmailUntilComplete = true, EmailFrequency = TimeSpan.FromMinutes(30), Flash = true, EmailRecipients = emailRecipients };
            routine.CreateConfiguration = new WorkflowStepConfiguration() { EmailOnStart = false, EmailOnCompletion = false, IncludeContentsInEmail = false, EmailRecipients = emailRecipients };
            routine.RegisterConfiguration = new WorkflowStepConfiguration() { EmailOnStart = false, EmailOnCompletion = false, IncludeContentsInEmail = false, EmailRecipients = emailRecipients };
            routine.ProcessConfiguration = new WorkflowStepConfiguration() { EmailOnStart = false, EmailOnCompletion = false, IncludeContentsInEmail = false, EmailRecipients = emailRecipients };
            SpecimenCategories.Instance[SpecimenCategories.ROUTINE_TITLE] = routine;
            
            Category timed = new Category() { Color = new SerializableColor(Color.Blue), Title = TIMED_TITLE, };
            timed.CheckPoint1Configuration = new CheckPointConfiguration() { Duration = TimeSpan.FromMinutes(15), EmailImmediately = false, EmailUntilComplete = false, EmailFrequency = TimeSpan.MaxValue, Flash = false, EmailRecipients = emailRecipients };
            timed.CheckPoint2Configuration = new CheckPointConfiguration() { Duration = TimeSpan.FromMinutes(30), EmailImmediately = false, EmailUntilComplete = false, EmailFrequency = TimeSpan.MaxValue, Flash = false, EmailRecipients = emailRecipients };
            timed.CheckPoint3Configuration = new CheckPointConfiguration() { Duration = TimeSpan.FromMinutes(45), EmailImmediately = false, EmailUntilComplete = false, EmailFrequency = TimeSpan.MaxValue, Flash = false, EmailRecipients = emailRecipients };
            timed.CheckPoint4Configuration = new CheckPointConfiguration() { Duration = TimeSpan.FromMinutes(60), EmailImmediately = true, EmailUntilComplete = true, EmailFrequency = TimeSpan.FromMinutes(15), Flash = true, EmailRecipients = emailRecipients };
            timed.CreateConfiguration = new WorkflowStepConfiguration() { EmailOnStart = false, EmailOnCompletion = false, IncludeContentsInEmail = false, EmailRecipients = emailRecipients };
            timed.RegisterConfiguration = new WorkflowStepConfiguration() { EmailOnStart = false, EmailOnCompletion = false, IncludeContentsInEmail = false, EmailRecipients = emailRecipients };
            timed.ProcessConfiguration = new WorkflowStepConfiguration() { EmailOnStart = false, EmailOnCompletion = false, IncludeContentsInEmail = false, EmailRecipients = emailRecipients };
            SpecimenCategories.Instance[SpecimenCategories.TIMED_TITLE] = timed;
            
            Category stat = new Category() { Color = new SerializableColor(Color.Pink), Title = STAT_TITLE, };
            stat.CheckPoint1Configuration = new CheckPointConfiguration() { Duration = TimeSpan.FromSeconds(45), EmailImmediately = false, EmailUntilComplete = false, EmailFrequency = TimeSpan.MaxValue, Flash = false, EmailRecipients = emailRecipients };
            stat.CheckPoint2Configuration = new CheckPointConfiguration() { Duration = TimeSpan.FromSeconds(50), EmailImmediately = false, EmailUntilComplete = false, EmailFrequency = TimeSpan.MaxValue, Flash = false, EmailRecipients = emailRecipients };
            stat.CheckPoint3Configuration = new CheckPointConfiguration() { Duration = TimeSpan.FromSeconds(55), EmailImmediately = false, EmailUntilComplete = false, EmailFrequency = TimeSpan.MaxValue, Flash = false, EmailRecipients = emailRecipients };
            stat.CheckPoint4Configuration = new CheckPointConfiguration() { Duration = TimeSpan.FromSeconds(60), EmailImmediately = true, EmailUntilComplete = true, EmailFrequency = TimeSpan.FromMinutes(15), Flash = true, EmailRecipients = emailRecipients };
            stat.CreateConfiguration = new WorkflowStepConfiguration() { EmailOnStart = false, EmailOnCompletion = false, IncludeContentsInEmail = false, EmailRecipients = emailRecipients };
            stat.RegisterConfiguration = new WorkflowStepConfiguration() { EmailOnStart = false, EmailOnCompletion = false, IncludeContentsInEmail = false, EmailRecipients = emailRecipients };
            stat.ProcessConfiguration = new WorkflowStepConfiguration() { EmailOnStart = false, EmailOnCompletion = false, IncludeContentsInEmail = false, EmailRecipients = emailRecipients };
            SpecimenCategories.Instance[SpecimenCategories.STAT_TITLE] = stat;
            
            Category problem = new Category() { Color = new SerializableColor(Color.Black), Title = PROBLEM_TITLE };
            problem.CheckPoint1Configuration = new CheckPointConfiguration() { Duration = TimeSpan.FromDays(1), EmailImmediately = true, EmailUntilComplete = true, EmailFrequency = TimeSpan.FromMinutes(360), Flash = false, EmailRecipients = emailRecipients };
            problem.CheckPoint2Configuration = new CheckPointConfiguration() { Duration = TimeSpan.FromDays(1), EmailImmediately = false, EmailUntilComplete = false, EmailFrequency = TimeSpan.MaxValue, Flash = false, EmailRecipients = emailRecipients };
            problem.CheckPoint3Configuration = new CheckPointConfiguration() { Duration = TimeSpan.FromDays(1), EmailImmediately = false, EmailUntilComplete = false, EmailFrequency = TimeSpan.MaxValue, Flash = false, EmailRecipients = emailRecipients };
            problem.CheckPoint4Configuration = new CheckPointConfiguration() { Duration = TimeSpan.FromDays(1), EmailImmediately = false, EmailUntilComplete = false, EmailFrequency = TimeSpan.FromMinutes(15), Flash = false, EmailRecipients = emailRecipients };
            problem.CreateConfiguration = new WorkflowStepConfiguration() { EmailOnStart = true, EmailOnCompletion = false, IncludeContentsInEmail = true, EmailRecipients = emailRecipients };
            problem.RegisterConfiguration = new WorkflowStepConfiguration() { EmailOnStart = false, EmailOnCompletion = false, IncludeContentsInEmail = false, EmailRecipients = emailRecipients };
            problem.ProcessConfiguration = new WorkflowStepConfiguration() { EmailOnStart = false, EmailOnCompletion = true, IncludeContentsInEmail = true, EmailRecipients = emailRecipients };
            SpecimenCategories.Instance[SpecimenCategories.PROBLEM_TITLE] = problem;
            */
        }
    }
}
