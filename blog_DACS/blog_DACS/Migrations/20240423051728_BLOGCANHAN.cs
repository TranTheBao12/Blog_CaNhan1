using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blog_DACS.Migrations
{
    /// <inheritdoc />
    public partial class BLOGCANHAN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    ID_Permission = table.Column<long>(type: "bigint", nullable: false),
                    Name_Permission = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description_Permission = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Permissi__D832E15CB1D2BBA0", x => x.ID_Permission);
                });

            migrationBuilder.CreateTable(
                name: "Role_User",
                columns: table => new
                {
                    ID_Role = table.Column<long>(type: "bigint", nullable: false),
                    Role_Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description_Role = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Role_Use__43DCD32D24310033", x => x.ID_Role);
                });

            migrationBuilder.CreateTable(
                name: "Role_Permission",
                columns: table => new
                {
                    ID_Role = table.Column<long>(type: "bigint", nullable: false),
                    ID_Permission = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Role_Per__9E5FFD383F15CA88", x => new { x.ID_Role, x.ID_Permission });
                    table.ForeignKey(
                        name: "FK_Role_Permission_Permission_ID",
                        column: x => x.ID_Permission,
                        principalTable: "Permission",
                        principalColumn: "ID_Permission");
                    table.ForeignKey(
                        name: "FK_Role_Permission_Role_ID",
                        column: x => x.ID_Role,
                        principalTable: "Role_User",
                        principalColumn: "ID_Role");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID_User = table.Column<long>(type: "bigint", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone_Number = table.Column<string>(type: "varchar(11)", unicode: false, maxLength: 11, nullable: true),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Date_Of_Birth = table.Column<DateTime>(type: "datetime", nullable: true),
                    Gender = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: true),
                    Place_Of_Birth = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Created_At = table.Column<DateTime>(type: "datetime", nullable: true),
                    Last_Updated_At = table.Column<DateTime>(type: "datetime", nullable: true),
                    Existence_Status = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Pass = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    ID_Role = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__ED4DE44262E4F214", x => x.ID_User);
                    table.ForeignKey(
                        name: "FK_Role_ID",
                        column: x => x.ID_Role,
                        principalTable: "Role_User",
                        principalColumn: "ID_Role");
                });

            migrationBuilder.CreateTable(
                name: "Favorite_User",
                columns: table => new
                {
                    ID_User = table.Column<long>(type: "bigint", nullable: false),
                    ID_Favorite_User = table.Column<long>(type: "bigint", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Favorite__E133B7A8FABD13D0", x => new { x.ID_User, x.ID_Favorite_User });
                    table.ForeignKey(
                        name: "FK_Favorite_User_Favorite_ID",
                        column: x => x.ID_Favorite_User,
                        principalTable: "Users",
                        principalColumn: "ID_User");
                    table.ForeignKey(
                        name: "FK_Favorite_User_User_ID",
                        column: x => x.ID_User,
                        principalTable: "Users",
                        principalColumn: "ID_User");
                });

            migrationBuilder.CreateTable(
                name: "Follow",
                columns: table => new
                {
                    ID_Follower = table.Column<long>(type: "bigint", nullable: false),
                    ID_Following = table.Column<long>(type: "bigint", nullable: false),
                    Existence_Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Follow__A54C4162780E0ED3", x => new { x.ID_Follower, x.ID_Following });
                    table.ForeignKey(
                        name: "FK_Follower_ID",
                        column: x => x.ID_Follower,
                        principalTable: "Users",
                        principalColumn: "ID_User");
                    table.ForeignKey(
                        name: "FK_Following_ID",
                        column: x => x.ID_Following,
                        principalTable: "Users",
                        principalColumn: "ID_User");
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    ID_Post = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Content_Post = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime", nullable: true),
                    Image_Post = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Last_Accessed_At = table.Column<DateTime>(type: "datetime", nullable: true),
                    Existence_Status = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: true),
                    ID_User = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Post__B41D0E30A485DD80", x => x.ID_Post);
                    table.ForeignKey(
                        name: "FK_Post_User_ID",
                        column: x => x.ID_User,
                        principalTable: "Users",
                        principalColumn: "ID_User");
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    ID_Post = table.Column<long>(type: "bigint", nullable: false),
                    ID_User = table.Column<long>(type: "bigint", nullable: false),
                    Content_Comment = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime", nullable: true),
                    Parent_Comment = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Comment__F60C34A10A92C3DF", x => new { x.ID_User, x.ID_Post });
                    table.ForeignKey(
                        name: "FK_Comment_Post_ID",
                        column: x => x.ID_Post,
                        principalTable: "Post",
                        principalColumn: "ID_Post");
                    table.ForeignKey(
                        name: "FK_Comment_User_ID",
                        column: x => x.ID_User,
                        principalTable: "Users",
                        principalColumn: "ID_User");
                });

            migrationBuilder.CreateTable(
                name: "Favorite_Post",
                columns: table => new
                {
                    ID_User = table.Column<long>(type: "bigint", nullable: false),
                    ID_Post = table.Column<long>(type: "bigint", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Favorite__F60C34A13C3C2CD1", x => new { x.ID_User, x.ID_Post });
                    table.ForeignKey(
                        name: "FK_Favorite_Post_Post_ID",
                        column: x => x.ID_Post,
                        principalTable: "Post",
                        principalColumn: "ID_Post");
                    table.ForeignKey(
                        name: "FK_Favorite_Post_User_ID",
                        column: x => x.ID_User,
                        principalTable: "Users",
                        principalColumn: "ID_User");
                });

            migrationBuilder.CreateTable(
                name: "Liked",
                columns: table => new
                {
                    ID_User = table.Column<long>(type: "bigint", nullable: false),
                    ID_Post = table.Column<long>(type: "bigint", nullable: false),
                    Created_At = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Liked__F60C34A102C6EE60", x => new { x.ID_User, x.ID_Post });
                    table.ForeignKey(
                        name: "FK_Thich_Post_ID",
                        column: x => x.ID_Post,
                        principalTable: "Post",
                        principalColumn: "ID_Post");
                    table.ForeignKey(
                        name: "FK_Thich_User_ID",
                        column: x => x.ID_User,
                        principalTable: "Users",
                        principalColumn: "ID_User");
                });

            migrationBuilder.CreateTable(
                name: "Share",
                columns: table => new
                {
                    ID_User = table.Column<long>(type: "bigint", nullable: false),
                    ID_Post = table.Column<long>(type: "bigint", nullable: false),
                    Shared_At = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Share__F60C34A1BC41E6B1", x => new { x.ID_User, x.ID_Post });
                    table.ForeignKey(
                        name: "FK_Share_Post_ID",
                        column: x => x.ID_Post,
                        principalTable: "Post",
                        principalColumn: "ID_Post");
                    table.ForeignKey(
                        name: "FK_Share_User_ID",
                        column: x => x.ID_User,
                        principalTable: "Users",
                        principalColumn: "ID_User");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ID_Post",
                table: "Comment",
                column: "ID_Post");

            migrationBuilder.CreateIndex(
                name: "IX_Favorite_Post_ID_Post",
                table: "Favorite_Post",
                column: "ID_Post");

            migrationBuilder.CreateIndex(
                name: "IX_Favorite_User_ID_Favorite_User",
                table: "Favorite_User",
                column: "ID_Favorite_User");

            migrationBuilder.CreateIndex(
                name: "IX_Follow_ID_Following",
                table: "Follow",
                column: "ID_Following");

            migrationBuilder.CreateIndex(
                name: "IX_Liked_ID_Post",
                table: "Liked",
                column: "ID_Post");

            migrationBuilder.CreateIndex(
                name: "IX_Post_ID_User",
                table: "Post",
                column: "ID_User");

            migrationBuilder.CreateIndex(
                name: "UQ__Post__B41D0E317F6B06DA",
                table: "Post",
                column: "ID_Post",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Role_Permission_ID_Permission",
                table: "Role_Permission",
                column: "ID_Permission");

            migrationBuilder.CreateIndex(
                name: "IX_Share_ID_Post",
                table: "Share",
                column: "ID_Post");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ID_Role",
                table: "Users",
                column: "ID_Role");

            migrationBuilder.CreateIndex(
                name: "UQ__Users__ED4DE443FA09E27B",
                table: "Users",
                column: "ID_User",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Favorite_Post");

            migrationBuilder.DropTable(
                name: "Favorite_User");

            migrationBuilder.DropTable(
                name: "Follow");

            migrationBuilder.DropTable(
                name: "Liked");

            migrationBuilder.DropTable(
                name: "Role_Permission");

            migrationBuilder.DropTable(
                name: "Share");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Role_User");
        }
    }
}
