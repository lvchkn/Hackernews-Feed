using Domain.Entities;

namespace Application.Tags;

public static class TagsSeeder
{
    public static List<Tag> GetSeedTags()
    {
        var categories = new List<TagCategory>
        {
            CreateAI(),
            CreateCloudPlatforms(),
            CreateDatabases(),
            CreateFrameworks(),
            CreateMobilePlatforms(),
            CreateProgrammingLanguages(),
            CreateScience(),
            CreateSecurity(),
            CreateStartup(),
            CreateSpace(),
            CreateWebDev(),
            CreateProgrammingTools(),
            CreateOS(),
            CreateIDE(),
            CreateCommunicationTools(),
            CreateBrowsers(),
            CreateCompanies(),
            CreateFinance(),
            CreateCareer(),
            CreateHardware(),
            CreateGameDev(),
            CreateMisc()
        };

        var tags = categories.SelectMany(category => category.TagNames.Select(tagName => new Tag
        {
            Name = tagName.CanonicalName,
            Category = category.CategoryName,
            Aliases = tagName.Aliases.Select(alias => new TagAlias { Name = alias }).ToList()
        })).ToList();

        return tags;
    }

    private static TagCategory CreateAI()
    {
        return new TagCategory(CategoryName: "AI",
            TagNames: [
                new TagName("AI", "Artificial Intelligence"),
                new TagName("Machine Learning", "ML"),
                new TagName("Deep Learning", "DL"),
                new TagName("Data Science"),
                new TagName("ChatGPT", "Chat GPT", "GPT", "GPT-4", "GPT-5", "GPT5", "o1", "o3"),
                new TagName("Copilot", "GitHub Copilot"),
                new TagName("Claude", "Claude Code", "Anthropic Claude"),
                new TagName("Gemini", "Google Gemini"),
                new TagName("LLM", "Large Language Models", "Large Language Model"),
                new TagName("Neural Network", "Neural Networks"),
                new TagName("NLP", "Natural Language Processing"),
                new TagName("Computer Vision"),
                new TagName("Deepseek", "DeepSeek"),
                new TagName("Stable Diffusion", "StableDiffusion"),
                new TagName("Midjourney", "MidJourney"),
                new TagName("DALL-E", "DALLE", "Dall-E"),
                new TagName("Hugging Face", "HuggingFace"),
                new TagName("Ollama"),
                new TagName("RAG", "Retrieval Augmented Generation"),
                new TagName("Fine-tuning", "Fine Tuning", "Finetuning"),
                new TagName("Prompt Engineering"),
                new TagName("AGI", "Artificial General Intelligence"),
                new TagName("Transformer", "Transformers"),
                new TagName("Llama", "LLaMA"),
                new TagName("Mistral"),
                new TagName("OpenRouter"),
                new TagName("Qwen"),
            ]
        );
    }

    private static TagCategory CreateCloudPlatforms()
    {
        return new TagCategory(CategoryName: "Cloud Platforms",
            TagNames: [
                new TagName("Cloud"),
                new TagName("AWS", "Amazon Web Services"),
                new TagName("Azure", "Microsoft Azure"),
                new TagName("GCP", "Google Cloud Platform", "Google Cloud"),
                new TagName("Cloudflare"),
                new TagName("Digital Ocean", "DigitalOcean"),
                new TagName("Vercel"),
                new TagName("Netlify"),
                new TagName("Heroku"),
                new TagName("Linode"),
                new TagName("Fly.io"),
            ]
        );
    }

    private static TagCategory CreateDatabases()
    {
        return new TagCategory(CategoryName: "Databases",
            TagNames: [
                new TagName("Database", "DB"),
                new TagName("SQL Server", "MSSQL", "Microsoft SQL Server"),
                new TagName("MySQL"),
                new TagName("PostgreSQL", "Postgres", "psql"),
                new TagName("MongoDB", "Mongo"),
                new TagName("Redis"),
                new TagName("SQLite"),
                new TagName("MariaDB"),
                new TagName("Elasticsearch", "ElasticSearch"),
                new TagName("DynamoDB", "Dynamo DB"),
                new TagName("CosmosDB", "Cosmos DB"),
                new TagName("OracleDB", "Oracle DB"),
                new TagName("Supabase"),
                new TagName("Cassandra"),
                new TagName("CouchDB"),
                new TagName("ClickHouse", "Clickhouse"),
                new TagName("CockroachDB"),
                new TagName("Neo4j"),
                new TagName("TimescaleDB"),
                new TagName("InfluxDB"),
            ]
        );
    }

    private static TagCategory CreateFrameworks()
    {
        return new TagCategory(CategoryName: "Frameworks",
            TagNames:
            [
                new TagName(".NET", "Dotnet", ".NET Core", "dotnet core", "dotnet"),
                new TagName("Node.js", "Node", "NodeJS"),
                new TagName("React", "ReactJS", "React.js"),
                new TagName("Express", "Express.js", "ExpressJS"),
                new TagName("Angular", "AngularJS"),
                new TagName("Next.js", "Next", "NextJS"),
                new TagName("ASP.NET", "ASPNET", "ASP.NET Core"),
                new TagName("Vue", "Vue.js", "VueJS"),
                new TagName("WordPress", "Wordpress"),
                new TagName("Flask"),
                new TagName("Spring", "Spring Boot", "SpringBoot"),
                new TagName("Django"),
                new TagName("Laravel"),
                new TagName("FastAPI"),
                new TagName("Ruby on Rails", "RubyOnRails", "Rails"),
                new TagName("Svelte", "SvelteKit"),
                new TagName("NestJS", "Nest"),
                new TagName("Blazor"),
                new TagName("Nuxt", "Nuxt.js", "NuxtJS"),
                new TagName("Symfony"),
                new TagName("Deno"),
                new TagName("Fastify"),
                new TagName("Phoenix"),
                new TagName("Bun"),
                new TagName("Remix"),
                new TagName("Astro"),
                new TagName("SolidJS", "Solid"),
                new TagName("Qwik"),
                new TagName("HTMX"),
            ]
        );
    }

    private static TagCategory CreateMobilePlatforms()
    {
        return new TagCategory(CategoryName: "Mobile",
            TagNames: [
                new TagName("Mobile"),
                new TagName("iOS", "iPhone", "iPad"),
                new TagName("Android"),
                new TagName("React Native"),
                new TagName("Flutter"),
                new TagName("SwiftUI"),
                new TagName("Kotlin Multiplatform", "KMP")
            ]
        );
    }

    private static TagCategory CreateProgrammingLanguages()
    {
        return new TagCategory(CategoryName: "Programming Languages",
            TagNames:
            [
                new TagName("C#", "CSharp", "C Sharp"),
                new TagName("Java"),
                new TagName("JavaScript", "Javascript", "JS"),
                new TagName("Go", "Golang"),
                new TagName("Rust"),
                new TagName("C++"),
                new TagName("C"),
                new TagName("Python"),
                new TagName("PowerShell", "Powershell"),
                new TagName("Bash", "Shell"),
                new TagName("Lua"),
                new TagName("F#", "FSharp"),
                new TagName("HTML", "HTML5"),
                new TagName("CSS", "CSS3"),
                new TagName("SQL"),
                new TagName("TypeScript", "Typescript", "TS"),
                new TagName("PHP", "Php"),
                new TagName("Kotlin"),
                new TagName("Ruby"),
                new TagName("Assembly", "ASM"),
                new TagName("Clojure"),
                new TagName("Haskell"),
                new TagName("Lisp", "Common Lisp"),
                new TagName("Delphi"),
                new TagName("MATLAB", "Matlab"),
                new TagName("Scala"),
                new TagName("R"),
                new TagName("Elixir"),
                new TagName("Swift"),
                new TagName("Erlang"),
                new TagName("Perl"),
                new TagName("Dart"),
                new TagName("Fortran"),
                new TagName("Visual Basic", "VisualBasic", "VB", "VB.NET"),
                new TagName("Zig"),
                new TagName("Nim"),
                new TagName("OCaml"),
                new TagName("Julia"),
                new TagName("Gleam"),
                new TagName("Mojo"),
                new TagName("Rust", "Rustlang"),
            ]
        );
    }

    private static TagCategory CreateScience()
    {
        return new TagCategory(CategoryName: "Science",
            TagNames:
            [
                new TagName("Science"),
                new TagName("Research"),
                new TagName("Physics"),
                new TagName("Biology"),
                new TagName("Chemistry"),
                new TagName("Mathematics", "Math", "Maths"),
                new TagName("Quantum Computing", "Quantum"),
            ]
        );
    }

    private static TagCategory CreateSecurity()
    {
        return new TagCategory(CategoryName: "Security",
            TagNames:
            [
                new TagName("Security", "Cybersecurity", "InfoSec"),
                new TagName("Hacking", "Hack"),
                new TagName("Privacy"),
                new TagName("Encryption"),
                new TagName("Vulnerability", "CVE"),
                new TagName("Malware"),
                new TagName("Ransomware"),
                new TagName("Phishing"),
                new TagName("Zero Day", "0-day"),
                new TagName("Penetration Testing", "Pentesting", "Pentest")
            ]
        );
    }

    private static TagCategory CreateStartup()
    {
        return new TagCategory(CategoryName: "Startup",
            TagNames:
            [
                new TagName("Startup", "Startups"),
                new TagName("Entrepreneurship"),
                new TagName("Entrepreneur"),
                new TagName("Founder", "Founder Story"),
                new TagName("YCombinator", "Y Combinator", "YC"),
                new TagName("Venture Capital", "VC"),
                new TagName("Fundraising"),
                new TagName("IPO"),
                new TagName("Acquisition")
            ]
        );
    }

    private static TagCategory CreateSpace()
    {
        return new TagCategory(CategoryName: "Space",
            TagNames:
            [
                new TagName("Space"),
                new TagName("Astronomy"),
                new TagName("NASA"),
                new TagName("SpaceX"),
                new TagName("Rocket"),
                new TagName("Satellite"),
                new TagName("Mars"),
                new TagName("Moon"),
            ]
        );
    }

    private static TagCategory CreateWebDev()
    {
        return new TagCategory(CategoryName: "Web Development",
            TagNames:
            [
                new TagName("WebDev", "Web Development", "Web Dev"),
                new TagName("Frontend", "Front-end", "Front End"),
                new TagName("Backend", "Back-end", "Back End"),
                new TagName("Full Stack", "Fullstack"),
                new TagName("DevOps"),
                new TagName("Design", "Web Design"),
                new TagName("UX", "User Experience"),
                new TagName("UI", "User Interface"),
                new TagName("Accessibility", "a11y"),
                new TagName("SEO", "Search Engine Optimization"),
                new TagName("PWA", "Progressive Web App")
            ]
        );
    }

    private static TagCategory CreateProgrammingTools()
    {
        return new TagCategory(CategoryName: "Programming Tools",
            TagNames:
            [
                new TagName("RabbitMQ"),
                new TagName("TensorFlow"),
                new TagName("Kafka", "Apache Kafka"),
                new TagName("Electron"),
                new TagName("PyTorch"),
                new TagName("Docker"),
                new TagName("npm"),
                new TagName("Yarn"),
                new TagName("Webpack"),
                new TagName("CMake"),
                new TagName("Kubernetes", "K8s"),
                new TagName("NuGet"),
                new TagName("Maven"),
                new TagName("Gradle"),
                new TagName("Vite"),
                new TagName("Homebrew", "brew"),
                new TagName("Ansible"),
                new TagName("Terraform"),
                new TagName("Cargo"),
                new TagName("Podman"),
                new TagName("pnpm"),
                new TagName("Logstash"),
                new TagName("Kibana"),
                new TagName("Traefik"),
                new TagName("Consul"),
                new TagName("Prometheus"),
                new TagName("Jaeger"),
                new TagName("Grafana"),
                new TagName("Nginx", "NGINX"),
                new TagName("ESLint"),
                new TagName("Git"),
                new TagName("scikit-learn", "sklearn", "scikit"),
                new TagName("Xamarin"),
                new TagName("OpenTelemetry", "OTel"),
                new TagName("Helm"),
                new TagName("ArgoCD", "Argo CD"),
                new TagName("Jenkins"),
                new TagName("CircleCI"),
                new TagName("GitHub Actions"),
                new TagName("Bazel"),
                new TagName("esbuild"),
                new TagName("Rollup"),
                new TagName("Parcel"),
                new TagName("Turborepo")
            ]
        );
    }

    private static TagCategory CreateOS()
    {
        return new TagCategory(CategoryName: "Operating Systems",
            TagNames:
            [
                new TagName("Linux"),
                new TagName("Windows"),
                new TagName("macOS", "MacOS", "Mac OS", "OSX", "OS X"),
                new TagName("Ubuntu"),
                new TagName("WSL", "Windows Subsystem for Linux"),
                new TagName("Debian"),
                new TagName("Red Hat", "RHEL", "RedHat"),
                new TagName("Fedora"),
                new TagName("Arch Linux", "Arch"),
                new TagName("NixOS", "Nix"),
                new TagName("FreeBSD"),
                new TagName("ChromeOS", "Chrome OS")
            ]
        );
    }

    private static TagCategory CreateIDE()
    {
        return new TagCategory(CategoryName: "IDEs & Editors",
            TagNames:
            [
                new TagName("VS Code", "VSCode", "Visual Studio Code"),
                new TagName("Visual Studio", "VisualStudio"),
                new TagName("IntelliJ", "IntelliJ IDEA"),
                new TagName("Vim"),
                new TagName("Neovim", "NeoVim", "nvim"),
                new TagName("Notepad++"),
                new TagName("Emacs"),
                new TagName("Sublime Text", "Sublime"),
                new TagName("JetBrains"),
                new TagName("Rider"),
                new TagName("PyCharm"),
                new TagName("WebStorm"),
                new TagName("Zed"),
                new TagName("Cursor"),
                new TagName("Antigravity")
            ]
        );
    }

    private static TagCategory CreateCommunicationTools()
    {
        return new TagCategory(CategoryName: "Communication Tools",
            TagNames:
            [
                new TagName("Azure DevOps", "ADO"),
                new TagName("Jira"),
                new TagName("Confluence"),
                new TagName("Trello"),
                new TagName("Microsoft Teams", "Teams", "MS Teams"),
                new TagName("Slack"),
                new TagName("Telegram"),
                new TagName("Discord"),
                new TagName("Zoom"),
                new TagName("Google Meet"),
                new TagName("Skype"),
                new TagName("Mattermost"),
                new TagName("WhatsApp"),
                new TagName("Linear"),
                new TagName("Notion"),
                new TagName("Obsidian")
            ]
        );
    }

    private static TagCategory CreateBrowsers()
    {
        return new TagCategory(CategoryName: "Browsers",
            TagNames:
            [
                new TagName("Chrome", "Google Chrome"),
                new TagName("Firefox", "Mozilla Firefox"),
                new TagName("Safari"),
                new TagName("Edge", "Microsoft Edge"),
                new TagName("Brave"),
                new TagName("Arc"),
                new TagName("Chromium")
            ]
        );
    }

    private static TagCategory CreateCompanies()
    {
        return new TagCategory(CategoryName: "Companies",
            TagNames:
            [
                new TagName("Google", "Alphabet"),
                new TagName("Amazon"),
                new TagName("Microsoft"),
                new TagName("Twitter", "X"),
                new TagName("Meta", "Facebook"),
                new TagName("Apple"),
                new TagName("Intel"),
                new TagName("AMD"),
                new TagName("Qualcomm"),
                new TagName("Netflix"),
                new TagName("OpenAI"),
                new TagName("Anthropic"),
                new TagName("Nvidia", "NVIDIA"),
                new TagName("Tesla"),
                new TagName("Stripe"),
                new TagName("Cloudflare"),
                new TagName("Vercel"),
                new TagName("GitHub"),
                new TagName("GitLab"),
                new TagName("Atlassian"),
                new TagName("JetBrains"),
                new TagName("HashiCorp"),
                new TagName("Databricks"),
                new TagName("Snowflake"),
                new TagName("Salesforce"),
                new TagName("Microsoft"),
                new TagName("Reddit"),
            ]
        );
    }

    private static TagCategory CreateFinance()
    {
        return new TagCategory(CategoryName: "Finance",
            TagNames:
            [
                new TagName("Fintech", "FinTech"),
                new TagName("Banking", "Bank"),
                new TagName("Payments", "Payment"),
                new TagName("Stock Market", "Stocks", "Stock"),
                new TagName("Trading"),
                new TagName("Investing", "Investment"),
                new TagName("FIRE", "Financial Independence"),
            ]
        );
    }

    private static TagCategory CreateCareer()
    {
        return new TagCategory(CategoryName: "Career",
            TagNames:
            [
                new TagName("Remote Work", "Remote", "WFH", "Work From Home"),
                new TagName("Hiring", "Jobs", "Job"),
                new TagName("Interview", "Tech Interview", "Interviews"),
                new TagName("Layoffs", "Layoff", "Laid Off"),
                new TagName("Salary", "Compensation", "Salaries"),
                new TagName("Work-Life Balance"),
                new TagName("Burnout"),
                new TagName("Freelance", "Freelancing", "Freelancer"),
                new TagName("Side Project", "Side Projects"),
                new TagName("Resume", "CV"),
                new TagName("Career Advice", "Career"),
                new TagName("Mentorship", "Mentor")
            ]
        );
    }

    private static TagCategory CreateHardware()
    {
        return new TagCategory(CategoryName: "Hardware",
            TagNames:
            [
                new TagName("Hardware"),
                new TagName("Raspberry Pi", "RaspberryPi", "RPi"),
                new TagName("Arduino"),
                new TagName("FPGA"),
                new TagName("GPU", "Graphics Card"),
                new TagName("CPU", "Processor"),
                new TagName("ARM", "ARM64"),
                new TagName("RISC-V", "RISCV"),
                new TagName("Embedded", "Embedded Systems"),
                new TagName("IoT", "Internet of Things"),
                new TagName("Chip", "Semiconductor", "Chips"),
                new TagName("3D Printing", "3D Printer"),
                new TagName("Robotics", "Robot", "Robots"),
                new TagName("Keyboard", "Mechanical Keyboard"),
                new TagName("SSD", "NVMe"),
                new TagName("RAM", "Memory")
            ]
        );
    }

    private static TagCategory CreateGameDev()
    {
        return new TagCategory(CategoryName: "Game Development",
            TagNames:
            [
                new TagName("Game Development", "Game Dev", "Gamedev", "GameDev"),
                new TagName("Unity", "Unity3D"),
                new TagName("Unreal Engine", "Unreal", "UE5", "UE4"),
                new TagName("Godot"),
                new TagName("Steam"),
                new TagName("Epic Games"),
                new TagName("PlayStation", "PS5", "PS4"),
                new TagName("Xbox"),
                new TagName("Nintendo", "Switch"),
                new TagName("VR", "Virtual Reality"),
                new TagName("AR", "Augmented Reality"),
                new TagName("Metaverse"),
                new TagName("Indie Game", "Indie Games", "Indie Dev"),
                new TagName("Game Engine", "Game Engines"),
                new TagName("OpenGL"),
                new TagName("Vulkan"),
                new TagName("DirectX"),
                new TagName("WebGL")
            ]
        );
    }

    private static TagCategory CreateMisc()
    {
        return new TagCategory(CategoryName: "Miscellaneous",
            TagNames:
            [
                new TagName("DDD", "Domain-Driven Design"),
                new TagName("Microservices", "Microservice"),
                new TagName("Clean Architecture"),
                new TagName("Design Patterns", "Design Pattern"),
                new TagName("Reverse Proxy"),
                new TagName("GraphQL"),
                new TagName("gRPC", "GRPC"),
                new TagName("Algorithm", "Algorithms"),
                new TagName("API", "APIs"),
                new TagName("CLI", "Command Line"),
                new TagName("Compiler", "Compilers"),
                new TagName("CI", "Continuous Integration"),
                new TagName("CD", "Continuous Deployment", "Continuous Delivery"),
                new TagName("Data Structures", "Data Structure"),
                new TagName("Deployment", "Deploy"),
                new TagName("Documentation", "Docs"),
                new TagName("Open Source", "Open source", "OSS"),
                new TagName("FOSS", "Free and Open Source"),
                new TagName("HTTP", "HTTPS"),
                new TagName("JSON"),
                new TagName("NoSQL"),
                new TagName("REST API", "REST", "RESTful"),
                new TagName("Server", "Servers"),
                new TagName("Serverless"),
                new TagName("Terminal"),
                new TagName("Web Components"),
                new TagName("WebAssembly", "WASM", "Wasm"),
                new TagName("WebSocket", "WebSockets"),
                new TagName("OAuth", "OAuth2"),
                new TagName("JWT", "JSON Web Token"),
                new TagName("YAML"),
                new TagName("Regex", "Regular Expression"),
                new TagName("Unicode"),
                new TagName("Monorepo"),
                new TagName("Testing", "Unit Test", "Unit Testing"),
                new TagName("TDD", "Test-Driven Development"),
                new TagName("Performance", "Perf"),
                new TagName("Caching", "Cache"),
                new TagName("Rate Limiting"),
                new TagName("Load Balancing", "Load Balancer"),
                new TagName("Self-hosted", "Self Hosted", "Selfhosted"),
                new TagName("Home Lab", "Homelab"),
                new TagName("Networking", "Network"),
                new TagName("DNS"),
                new TagName("SSL", "TLS"),
                new TagName("Markdown", "MD"),
                new TagName("RSS", "Atom"),
                new TagName("Email", "SMTP"),
                new TagName("Backup", "Backups"),
                new TagName("Migration"),
                new TagName("Refactoring", "Refactor"),
                new TagName("Code Review"),
                new TagName("Debugging", "Debug"),
                new TagName("Logging", "Logs")
            ]
        );
    }
}
