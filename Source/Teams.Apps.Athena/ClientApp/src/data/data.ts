export const taxonomy: any[] = [
    {
        Task_ID: 2,
        Name: "Medical Studies",
        Type: "Folder",
        Status: "Status",
        Organization: "Organization"
    },
    {
        Task_ID: 3,
        Task_Parent_ID: 2,
        Name: "Infectious disease",
        Type: "Folder",
        Status: "Status",
        Organization: "Organization"
    },
    {
        Task_ID: 4,
        Task_Parent_ID: 2,
        Name: "Nutrition",
        Type: "Folder",
        Status: "Status",
        Organization: "Organization"
    },
    {
        Task_ID: 5,
        Name: "Political Science",
        Type: "Folder",
        Status: "Status",
        Organization: "Organization"
    },
    {
        Task_ID: 6,
        Task_Parent_ID: 5,
        Name: "Geopolitics",
        Type: "Folder",
        Status: "Status",
        Organization: "Organization"
    }
];

export enum EntityType {
    ResearchPaper,
    ResearchRequest,
    News,
    CommunityOfInterest,
    User
}

export enum StatusType {
    None,
    Proposed,
    InProgress,
    Completed
}

export const researchProjectData = [
    { "Id": "65a51902-13f6-46f5-b8a1-786c019ade43", "Title": "Best nutritional intake to improve health", "Keywords": ["Nutrition"], "Geography": "United States (US)", "Status": StatusType.Proposed, "Type": EntityType.ResearchRequest, "Comments": [{ "Name": "Drek T", "Comment": "Nice topic!" }] },
    { "Id": "1bd44f14-71fa-46d7-b130-71652fa46454", "Title": "Improve system performance in distributed systems", "Keywords": ["Distributed Design"], "Geography": "India", "Status": StatusType.InProgress, "Type": EntityType.ResearchPaper, "Comments": [{ "Name": "Drek T", "Comment": "Nice topic!" }, { "Name": "James Hulk", "Comment": "Nice topic!" }] },
    { "Id": "3a90a74d-7e46-41a2-950c-12ac0f88ca2b", "Title": "Measuring and improving cloud health", "Keywords": ["Cloud Computing"], "Geography": "India", "Status": StatusType.Completed, "Type": EntityType.ResearchPaper, "Comments": [{ "Name": "Tim j", "Comment": "Nice topic!" }, { "Name": "Harry P", "Comment": "Nice topic!" }] },
    { "Id": "938f4174-4726-4640-a7f0-10d7c3b06034", "Title": "Measuring and improving cloud health", "Keywords": ["Cloud Computing", "Cloud"], "Geography": "United States (US)", "Status": StatusType.Proposed, "Type": EntityType.ResearchPaper, "Comments": [] },
    { "Id": "49f5b876-eac2-43e6-be76-2d5ae636b1b3", "Title": "Identifying cloud's most unused resources", "Keywords": ["Cloud Computing"], "Geography": "United States (US)", "Status": StatusType.Proposed, "Type": EntityType.ResearchRequest, "Comments": [] },
    { "Id": "febfc2e7-0e9a-4a22-b0d2-a1bc3685d0ac", "Title": "Identifying and allocating distributed machines which are in ideal state", "Keywords": ["Distributed Design"], "Geography": "United States (US)", "Status": StatusType.Completed, "Type": EntityType.ResearchPaper, "Comments": [{ "Name": "Sherlock H", "Comment": "Nice topic!" }, { "Name": "Steve R", "Comment": "Nice topic!" }] },
    { "Id": "d3645b24-06ff-419e-be99-c616d8d4dea0", "Title": "Manage and design distributed design in efficient way", "Keywords": ["Distributed Design"], "Geography": "India", "Status": StatusType.Proposed, "Type": EntityType.ResearchPaper, "Comments": [{ "Name": "Robert J", "Comment": "Nice topic!" }, { "Name": "Anna H", "Comment": "Nice topic!" }] },
    { "Id": "ac16452d-bd89-4ac1-838f-953957c888f7", "Title": "Identifying reasons of spreading of infection and minimizing it", "Keywords": ["Infectious disease"], "Geography": "India", "Status": StatusType.Completed, "Type": EntityType.ResearchPaper, "Comments": [{ "Name": "Jim J", "Comment": "Nice topic!" }, { "Name": "Nikki T", "Comment": "Nice topic!" }, { "Name": "Jenna R", "Comment": "Nice topic!" }] },
    { "Id": "11f6add6-8037-437a-83d4-6b7ae6eb9061", "Title": "Identifying reasons of spreading of infection and minimizing it", "Keywords": ["Infectious disease"], "Geography": "India", "Status": StatusType.InProgress, "Type": EntityType.ResearchRequest, "Comments": [] },
    { "Id": "c29f3c50-6394-4448-8793-cc34f44bca5f", "Title": "Storing weapon information on cloud secretly!", "Keywords": ["Cloud Computing", "Cloud"], "Geography": "India", "Status": StatusType.None, "Type": EntityType.News, "Comments": [{ "Name": "Tom H", "Comment": "Nice topic!" }] }
];
