using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ideastrike.Nancy.Helpers;

namespace Ideastrike.Nancy.Models.ViewModels
{
    public class IdeaViewModel
    {
        public IdeaViewModel(Idea idea)
        {
            Id = idea.Id;
            Title = idea.Title;
            Status = idea.Status;
            Time = idea.Time;
            Description = MarkdownHelper.Markdown(idea.Description);
            UserHasVoted = idea.UserHasVoted;
            TotalVotes = idea.Votes.Count;
            Author = idea.Author;
            GravatarUrl = (string.IsNullOrEmpty(Author.AvatarUrl)) ? Author.Email.ToGravatarUrl(40) : Author.AvatarUrl;
            Features = idea.Features.Select(f => new FeatureViewModel(f)).ToList();
            Activities = idea.Activities.Select(f => new ActivityViewModel(f)).ToList();

            Images = idea.Images.ToList();
        }

        public IEnumerable<Image> Images { get; set; }

        public IEnumerable<FeatureViewModel> Features { get; set; }
        
        [Obsolete("Make a secondary call to fetch these and render dynamically")]
        public IEnumerable<ActivityViewModel> Activities { get; set; }

        public string GravatarUrl { get; private set; }
        public DateTime Time { get; private set; }
        public User Author { get; private set; }
        public bool UserHasVoted { get; set; }
        public int TotalVotes { get; private set; }
        public string Title { get; private set; }
        public string Status { get; private set; }
        public int Id { get; private set; }
        public IHtmlString Description { get; private set; }
    }
}