using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "interests",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_interests", x => x.id);
                });
            
            migrationBuilder.CreateTable(
                name: "stories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    by = table.Column<string>(type: "text", nullable: false),
                    descendants = table.Column<int>(type: "integer", nullable: false),
                    kids = table.Column<int[]>(type: "integer[]", nullable: false),
                    score = table.Column<int>(type: "integer", nullable: false),
                    time = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false),
                    text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stories", x => x.id);
                });
            
            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tags", x => x.id);
                });
            
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    last_active = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });
            
            migrationBuilder.CreateTable(
                name: "story_tag",
                columns: table => new
                {
                    stories_id = table.Column<int>(type: "integer", nullable: false),
                    tags_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_story_tag", x => new { x.stories_id, x.tags_id });
                    table.ForeignKey(
                        name: "fk_story_tag_stories_stories_id",
                        column: x => x.stories_id,
                        principalTable: "stories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_story_tag_tags_tags_id",
                        column: x => x.tags_id,
                        principalTable: "tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
            
            migrationBuilder.CreateTable(
                name: "interest_user",
                columns: table => new
                {
                    interested_users_id = table.Column<int>(type: "integer", nullable: false),
                    interests_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_interest_user", x => new { x.interested_users_id, x.interests_id });
                    table.ForeignKey(
                        name: "fk_interest_user_interests_interests_id",
                        column: x => x.interests_id,
                        principalTable: "interests",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_interest_user_users_interested_users_id",
                        column: x => x.interested_users_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
            
            migrationBuilder.CreateTable(
                name: "story_user",
                columns: table => new
                {
                    favourite_stories_id = table.Column<int>(type: "integer", nullable: false),
                    favourited_by_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_story_user", x => new { x.favourite_stories_id, x.favourited_by_id });
                    table.ForeignKey(
                        name: "fk_story_user_stories_favourite_stories_id",
                        column: x => x.favourite_stories_id,
                        principalTable: "stories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_story_user_users_favourited_by_id",
                        column: x => x.favourited_by_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
            
            migrationBuilder.CreateIndex(
                name: "ix_interest_user_interests_id",
                table: "interest_user",
                column: "interests_id");
            
            migrationBuilder.CreateIndex(
                name: "ix_story_tag_tags_id",
                table: "story_tag",
                column: "tags_id");
            
            migrationBuilder.CreateIndex(
                name: "ix_story_user_favourited_by_id",
                table: "story_user",
                column: "favourited_by_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "interest_user");
            
            migrationBuilder.DropTable(
                name: "story_tag");
            
            migrationBuilder.DropTable(
                name: "story_user");
            
            migrationBuilder.DropTable(
                name: "interests");
            
            migrationBuilder.DropTable(
                name: "tags");
            
            migrationBuilder.DropTable(
                name: "stories");
            
            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
