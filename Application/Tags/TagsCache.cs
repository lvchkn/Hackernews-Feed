using System.Collections.ObjectModel;

namespace Application.Tags;

public class TagsCache
{
    private readonly List<string> _tags = new();
    
    public ReadOnlyCollection<string> Tags => _tags.AsReadOnly();

    public TagsCache()
    {
        AddAI();
        AddCloudPlatforms();
        AddCrypto();

        AddDatabases();
        AddFrameworks();
        AddMobilePlatforms();

        AddProgrammingLanguages();
        AddScience();
        AddSecurity();

        AddStartup();
        AddSpace();
        AddWebDev();
        
        AddProgrammingTools();
        AddOS();
        AddIDE();
        
        AddCommunicationTools();
        AddBrowsers();
        AddCompanies();
        
        AddMisc();
    }

    private void AddAI()
    {
        var ai = new[]
        {
            "AI",
            "Machine Learning",
            "Deep Learning",
            "Data Science",
            "Chat GPT",
            "ChatGPT",
            "Copilot",
            "Tabnine",
        };

        _tags.AddRange(ai);
    }

    private void AddCloudPlatforms()
    {
        var cloudPlatforms = new[]
        {
            "Cloud",
            "AWS",
            "Azure",
            "GCP",
            "Cloudflare",
            "Digital Ocean",
            "Vercel",
            "Netlify",
        };

        _tags.AddRange(cloudPlatforms);
    }

    private void AddCrypto()
    {
        var crypto = new[]
        {
            "Crypto",
            "Bitcoin",
            "Ethereum",
            "Blockchain",
        };

        _tags.AddRange(crypto);
    }

    private void AddDatabases()
    {
        var databases = new[]
        {
            "Database",
            "MSSQL",
            "MySQL",
            "Postgres",
            "MongoDB",
            "Redis",
            "SQLite",
            "MariaDB",
            "ElasticSearch",
            "DynamoDB",
            "CosmosDB",
            "Oracle",
            "Supabase",
            "Cassandra",
            "CouchDB",
            "Clickhouse",
            "CockroachDB",
        };

        _tags.AddRange(databases);
    }

    private void AddFrameworks()
    {
        var frameworks = new[]
        {
            "Framework",
            "Dotnet",
            "Node",
            "React",
            "JQuery",
            "Express",
            "Angular",
            "Next",
            "ASPNET",
            "Vue",
            "Wordpress",
            "Flask",
            "Spring",
            "Django",
            "Laravel",
            "FastAPI",
            "RubyOnRails",
            "Svelte",
            "Nest",
            "Blazor",
            "Nuxt",
            "Symfony",
            "Deno",
            "Fastify",
            "Phoenix",
            "Bun",
        };

        _tags.AddRange(frameworks);
    }

    private void AddMobilePlatforms()
    {
        var mobilePlatforms = new[]
        {
            "Mobile",
            "iOS",
            "Android",
        };

        _tags.AddRange(mobilePlatforms);
    }

    private void AddProgrammingLanguages()
    {
        var languages = new[]
        {
            "ProgrammingLanguage",
            "C#",
            "Java",
            "Javascript",
            "Golang",
            "Go",
            "Rust",
            "C++",
            "C",
            "Python",
            "Powershell",
            "Bash",
            "Lua",
            "F#",
            "HTML",
            "CSS",
            "SQL",
            "Typescript",
            "Php",
            "Kotlin",
            "Ruby",
            "Assembly",
            "Clojure",
            "Haskell",
            "Lisp",
            "Delphi",
            "Matlab",
            "Scala",
            "R",
            "Elixir",
            "Swift",
            "Erlang",
            "Perl",
            "Dart",
            "Fortran",
            "VisualBasic",
        };

        _tags.AddRange(languages);
    }

    private void AddScience()
    {
        var science = new[]
        {
            "Science",
            "Research",
        };

        _tags.AddRange(science);
    }

    private void AddSecurity()
    {
        var security = new[]
        {
            "Security",
            "Hacking",
            "Privacy",
        };

        _tags.AddRange(security);
    }

    private void AddStartup()
    {
        var startupBuzzwords = new[]
        {
            "Startup",
            "Entrepreneurship",
            "Entrepreneur",
            "Founder Story",
        };

        _tags.AddRange(startupBuzzwords);
    }

    private void AddSpace()
    {
        var space = new[]
        {
            "Space",
            "Astronomy",
        };

        _tags.AddRange(space);
    }

    private void AddWebDev()
    {
        var webDev = new[]
        {
            "WebDev",
            "Frontend",
            "Backend",
            "DevOps",
            "Design",
        };

        _tags.AddRange(webDev);
    }

    private void AddProgrammingTools()
    {
        var programmingTools = new[]
        {
            "RabbitMQ",
            "TensorFlow",
            "Flutter",
            "Kafka",
            "Electron",
            "PyTorch",
            "Docker",
            "npm",
            "yarn",
            "Webpack",
            "CMake",
            "Kubernetes",
            "NuGet",
            "Maven",
            "Gradle",
            "Vite",
            "Homebrew",
            "Ansible",
            "Terraform",
            "Cargo",
            "Podman",
            "pnpm",
            "Logstash",
            "Kibana",
            "Traefik",
            "Consul",
            "Prometheus",
            "Jaeger",
            "Grafana",
            "Nginx",
            "ESLint",
            "Git",
            "scikit-learn",
            "scikit",
            "Xamarin",
        };

        _tags.AddRange(programmingTools);
    }

    private void AddOS()
    {
        var os = new[]
        {
            "Linux",
            "Windows",
            "MacOS",
            "Ubuntu",
            "WSL",
            "Debian",
            "Red Hat",
        };

        _tags.AddRange(os);
    }

    private void AddIDE()
    {
        var ide = new[]
        {
            "VS Code",
            "VisualStudio",
            "IntelliJ",
            "Vim",
            "Neovim",
            "Notepad++",
        };

        _tags.AddRange(ide);
    }

    private void AddCommunicationTools()
    {
        var communicationTools = new[]
        {
            "Azure DevOps",
            "Jira",
            "Confluence",
            "Trello",
            "Microsoft Teams",
            "Slack",
            "Telegram",
            "Discord",
            "Zoom",
            "Google Meet",
            "Skype",
            "Mattermost",
            "WhatsApp",
        };

        _tags.AddRange(communicationTools);
    }

    private void AddBrowsers()
    {
        var browsers = new[]
        {
            "Chrome",
            "Firefox",
            "Safari",
        };

        _tags.AddRange(browsers);
    }

    private void AddCompanies()
    {
        var companies = new[]
        {
            "Google",
            "Amazon",
            "Microsoft",
            "Twitter",
        };

        _tags.AddRange(companies);
    }

    private void AddMisc()
    {
        var misc = new[]
        {
            "DDD",
            "Microservices",
            "Clean architecture",
            "Design patterns",
            "Reverse proxy",
            "GraphQL",
            "GRPC",
            "Algorithm",
            "API",
            "Arduino",
            "CLI",
            "Compiler",
            "Continuous Integration",
            "Continuous Deployment",
            "Data Structures",
            "Deployment",
            "Documentation",
            "Github",
            "Open source",
            "FOSS",
            "HTTP",
            "JSON",
            "NoSQL",
            "Raspberry Pi",
            "REST API",
            "Server",
            "Serverless",
            "Terminal",
            "Web Components",
        };

        _tags.AddRange(misc);
    }
}