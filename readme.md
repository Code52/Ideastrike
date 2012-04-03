## Ideastrike

# This repository is temporarily frozen (think carbonite)

After much deliberation and agonizing, we have decided to take a fresh start to Ideastrike and switch to using MVC4. 

The repository is [here](https://github.com/shiftkey/thoughtstrike) and once we are happy with the transition we will bring the changes across to here.

So if you would like to add features to Ideastrike, please use the other repository. We will not be accepting new pull requests on this repository in the short term.

Regards,

@shiftkey



### About

We want a tool where people can suggest ideas and others can actively refine and enhance the ideas over time. 

Once an idea gets enough interest to kick start a project, the other systems that we use  - at this point in time GitHub and Trello - should integrate nicely to automate the mundane tasks of project management.

The existing tools out there are decent at what they do, but we want something more customisable and extensible.

### Features

 * The website uses NancyFx - hosted on ASP.NET but can be supported on any OWIN-capable host.
 * User authentication via Janrain (RPXNow) - use Google/Facebook/Twitter to sign in
 * Markdown support just about everywhere on the site. And a preview mode too, using Showdown.
 * Twitter Bootstrap has been used as the baseline for our site theme.
 * Image Uploading using the jQuery File Upload plugin.
 * AppHarbor support 

### Getting started

**Getting started with Git and GitHub**

 * [Setting up Git for Windows and connecting to GitHub](http://help.github.com/win-set-up-git/)
 * [Forking a GitHub repository](http://help.github.com/fork-a-repo/)
 * [The simple gude to GIT guide](http://rogerdudler.github.com/git-guide/)

Once you're familiar with Git and GitHub, clone the repository and run the ```.\build.cmd``` script to compile the code and run all the unit tests. You can use this script to test your changes quickly.

### Discussing ideas 

* [Trello Board](https://trello.com/board/ideastrike/4f137b417201526045146b8a) - add ideas, or claim an idea and start working on it!
* [JabbR Chatroom](http://jabbr.net/#/rooms/Ideastrike) - discuss things in real-time with people all over the world!

### Libraries used

 * NancyFx
 * Entity Framework 4.3-beta1 - with Code Migrations