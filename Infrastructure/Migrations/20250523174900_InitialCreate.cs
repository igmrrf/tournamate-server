using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Fouls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    YellowCards = table.Column<int>(type: "int", nullable: false),
                    RedCards = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<Guid>(type: "char(36)", nullable: false),
                    PlayerId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fouls", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Performance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MatchesPlayed = table.Column<int>(type: "int", nullable: false),
                    Wins = table.Column<int>(type: "int", nullable: false),
                    Draws = table.Column<int>(type: "int", nullable: false),
                    Losses = table.Column<int>(type: "int", nullable: false),
                    Diff = table.Column<int>(type: "int", nullable: false),
                    TotalPoint = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Performance", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    HomeTeamScore = table.Column<int>(type: "int", nullable: false),
                    AwayTeamScore = table.Column<int>(type: "int", nullable: false),
                    IsPernaltyShootout = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ScorerId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ScorerTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TeamId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Subtitues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TeamId = table.Column<Guid>(type: "char(36)", nullable: false),
                    PlayerInId = table.Column<Guid>(type: "char(36)", nullable: false),
                    PlayerOutId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subtitues", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TournamentInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Information = table.Column<string>(type: "longtext", nullable: false),
                    SportName = table.Column<string>(type: "longtext", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CheckInDuration = table.Column<int>(type: "int", nullable: true),
                    DurationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsPrivate = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TounamentThumbnail = table.Column<string>(type: "longtext", nullable: true),
                    CreatedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentInfos", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserName = table.Column<string>(type: "longtext", nullable: false),
                    Country = table.Column<string>(type: "longtext", nullable: false),
                    Email = table.Column<string>(type: "longtext", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: false),
                    PasswordHashSalt = table.Column<string>(type: "longtext", nullable: false),
                    IsVerified = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    VerificationToken = table.Column<string>(type: "longtext", nullable: true),
                    VerificationTokenExpiry = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    PasswordResetToken = table.Column<string>(type: "longtext", nullable: true),
                    PasswordResetTokenExpiry = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RefreshTokenExpiration = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    RefreshToken = table.Column<string>(type: "longtext", nullable: false),
                    CreatedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Tournament",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    CurrentPlayerCount = table.Column<int>(type: "int", nullable: false),
                    CurrentTeamCount = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    TournamentInfoId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    InvitationCode = table.Column<string>(type: "longtext", nullable: false),
                    TournamentMode = table.Column<int>(type: "int", nullable: false),
                    TournamentType = table.Column<int>(type: "int", nullable: false),
                    NoOfPlayers = table.Column<int>(type: "int", nullable: false),
                    NoOfTeams = table.Column<int>(type: "int", nullable: false),
                    NoOfSubPlayers = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournament", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tournament_TournamentInfos_TournamentInfoId",
                        column: x => x.TournamentInfoId,
                        principalTable: "TournamentInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Invitations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TournamentId = table.Column<Guid>(type: "char(36)", nullable: false),
                    SenderId = table.Column<Guid>(type: "char(36)", nullable: false),
                    InvitationStatus = table.Column<int>(type: "int", nullable: false),
                    RecipientId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    InvitationCode = table.Column<string>(type: "longtext", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invitations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TournamentId = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Role_Tournament_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournament",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Role_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Round",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    RoundNumber = table.Column<int>(type: "int", nullable: false),
                    TournamentId = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Round", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Round_Tournament_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournament",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TournamentId = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CanUpdateScore = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanRecordFoul = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanMakeSubstitutions = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CanUpdateTimeStamp = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TournamentRoleId = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permission_Role_TournamentRoleId",
                        column: x => x.TournamentRoleId,
                        principalTable: "Role",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Permission_Tournament_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournament",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Match",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    TournamentId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Round = table.Column<int>(type: "int", nullable: false),
                    HomeId = table.Column<Guid>(type: "char(36)", nullable: false),
                    AwayId = table.Column<Guid>(type: "char(36)", nullable: false),
                    MatchStatus = table.Column<int>(type: "int", nullable: false),
                    MatchScoreId = table.Column<Guid>(type: "char(36)", nullable: true),
                    MatchSubstitutionsId = table.Column<Guid>(type: "char(36)", nullable: true),
                    MatchFoulsId = table.Column<Guid>(type: "char(36)", nullable: false),
                    TeamPerformanceId = table.Column<Guid>(type: "char(36)", nullable: false),
                    TournamentRoundId = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Match", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Match_Fouls_MatchFoulsId",
                        column: x => x.MatchFoulsId,
                        principalTable: "Fouls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Match_Performance_TeamPerformanceId",
                        column: x => x.TeamPerformanceId,
                        principalTable: "Performance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Match_Round_TournamentRoundId",
                        column: x => x.TournamentRoundId,
                        principalTable: "Round",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Match_Scores_MatchScoreId",
                        column: x => x.MatchScoreId,
                        principalTable: "Scores",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Match_Subtitues_MatchSubstitutionsId",
                        column: x => x.MatchSubstitutionsId,
                        principalTable: "Subtitues",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Match_Tournament_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournament",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Logo = table.Column<string>(type: "longtext", nullable: true),
                    NoOfPlayer = table.Column<int>(type: "int", nullable: false),
                    NoOfSubPlayer = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    TournamentId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Code = table.Column<string>(type: "longtext", nullable: false),
                    TournamentRoundId = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Team_Round_TournamentRoundId",
                        column: x => x.TournamentRoundId,
                        principalTable: "Round",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Team_Tournament_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournament",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MatchTime",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    HasMatchbegan = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HasMatchEnded = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    WhoWon = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    MatchId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchTime", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchTime_Match_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Match",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    TOurnmaentId = table.Column<Guid>(type: "char(36)", nullable: false),
                    TeamId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Position = table.Column<string>(type: "longtext", nullable: false),
                    JerseyNumber = table.Column<string>(type: "longtext", nullable: false),
                    NoOfPlayer = table.Column<int>(type: "int", nullable: false),
                    IsAssinged = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TournamentId = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedById = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Player_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Player_Tournament_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournament",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_UserId",
                table: "Invitations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_MatchFoulsId",
                table: "Match",
                column: "MatchFoulsId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_MatchScoreId",
                table: "Match",
                column: "MatchScoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_MatchSubstitutionsId",
                table: "Match",
                column: "MatchSubstitutionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_TeamPerformanceId",
                table: "Match",
                column: "TeamPerformanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_TournamentId",
                table: "Match",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_TournamentRoundId",
                table: "Match",
                column: "TournamentRoundId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchTime_MatchId",
                table: "MatchTime",
                column: "MatchId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permission_TournamentId",
                table: "Permission",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Permission_TournamentRoleId",
                table: "Permission",
                column: "TournamentRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_TeamId",
                table: "Player",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_TournamentId",
                table: "Player",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_TournamentId",
                table: "Role",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Role_UserId",
                table: "Role",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Round_TournamentId",
                table: "Round",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_TournamentId",
                table: "Team",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_TournamentRoundId",
                table: "Team",
                column: "TournamentRoundId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_TournamentInfoId",
                table: "Tournament",
                column: "TournamentInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.DropTable(
                name: "MatchTime");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "Match");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "Fouls");

            migrationBuilder.DropTable(
                name: "Performance");

            migrationBuilder.DropTable(
                name: "Scores");

            migrationBuilder.DropTable(
                name: "Subtitues");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Round");

            migrationBuilder.DropTable(
                name: "Tournament");

            migrationBuilder.DropTable(
                name: "TournamentInfos");
        }
    }
}
