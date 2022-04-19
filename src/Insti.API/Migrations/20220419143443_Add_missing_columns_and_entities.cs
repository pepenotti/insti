using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Insti.API.Migrations
{
    public partial class Add_missing_columns_and_entities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assistances_Asignatures_AsignatureId",
                table: "Assistances");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Asignatures_AsignatureId",
                table: "Schedules");

            migrationBuilder.DropTable(
                name: "AsignatureProfessor");

            migrationBuilder.DropTable(
                name: "Asignatures");

            migrationBuilder.RenameColumn(
                name: "AsignatureId",
                table: "Schedules",
                newName: "ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_Schedules_AsignatureId",
                table: "Schedules",
                newName: "IX_Schedules_ModuleId");

            migrationBuilder.RenameColumn(
                name: "AsignatureId",
                table: "Assistances",
                newName: "ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_Assistances_AsignatureId",
                table: "Assistances",
                newName: "IX_Assistances_ModuleId");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Genders",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Courses",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Courses",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "StudyPlan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Summary = table.Column<string>(type: "TEXT", nullable: false),
                    Goals = table.Column<string>(type: "TEXT", nullable: false),
                    TimeFrame = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyPlan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    StudyPlanId = table.Column<int>(type: "INTEGER", nullable: false),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Modules_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Modules_StudyPlan_StudyPlanId",
                        column: x => x.StudyPlanId,
                        principalTable: "StudyPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudyPlanContent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    StudyPlanId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyPlanContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyPlanContent_StudyPlan_StudyPlanId",
                        column: x => x.StudyPlanId,
                        principalTable: "StudyPlan",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ModuleProfessor",
                columns: table => new
                {
                    ModulesId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProfessorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleProfessor", x => new { x.ModulesId, x.ProfessorId });
                    table.ForeignKey(
                        name: "FK_ModuleProfessor_Modules_ModulesId",
                        column: x => x.ModulesId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModuleProfessor_Professors_ProfessorId",
                        column: x => x.ProfessorId,
                        principalTable: "Professors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudyPlanContentData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Data = table.Column<string>(type: "TEXT", nullable: false),
                    StudyPlanContentId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyPlanContentData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudyPlanContentData_StudyPlanContent_StudyPlanContentId",
                        column: x => x.StudyPlanContentId,
                        principalTable: "StudyPlanContent",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModuleProfessor_ProfessorId",
                table: "ModuleProfessor",
                column: "ProfessorId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_CourseId",
                table: "Modules",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_StudyPlanId",
                table: "Modules",
                column: "StudyPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlanContent_StudyPlanId",
                table: "StudyPlanContent",
                column: "StudyPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_StudyPlanContentData_StudyPlanContentId",
                table: "StudyPlanContentData",
                column: "StudyPlanContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assistances_Modules_ModuleId",
                table: "Assistances",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Modules_ModuleId",
                table: "Schedules",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assistances_Modules_ModuleId",
                table: "Assistances");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedules_Modules_ModuleId",
                table: "Schedules");

            migrationBuilder.DropTable(
                name: "ModuleProfessor");

            migrationBuilder.DropTable(
                name: "StudyPlanContentData");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropTable(
                name: "StudyPlanContent");

            migrationBuilder.DropTable(
                name: "StudyPlan");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Genders");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "ModuleId",
                table: "Schedules",
                newName: "AsignatureId");

            migrationBuilder.RenameIndex(
                name: "IX_Schedules_ModuleId",
                table: "Schedules",
                newName: "IX_Schedules_AsignatureId");

            migrationBuilder.RenameColumn(
                name: "ModuleId",
                table: "Assistances",
                newName: "AsignatureId");

            migrationBuilder.RenameIndex(
                name: "IX_Assistances_ModuleId",
                table: "Assistances",
                newName: "IX_Assistances_AsignatureId");

            migrationBuilder.CreateTable(
                name: "Asignatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asignatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Asignatures_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AsignatureProfessor",
                columns: table => new
                {
                    AsignaturesId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProfessorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsignatureProfessor", x => new { x.AsignaturesId, x.ProfessorId });
                    table.ForeignKey(
                        name: "FK_AsignatureProfessor_Asignatures_AsignaturesId",
                        column: x => x.AsignaturesId,
                        principalTable: "Asignatures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AsignatureProfessor_Professors_ProfessorId",
                        column: x => x.ProfessorId,
                        principalTable: "Professors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AsignatureProfessor_ProfessorId",
                table: "AsignatureProfessor",
                column: "ProfessorId");

            migrationBuilder.CreateIndex(
                name: "IX_Asignatures_CourseId",
                table: "Asignatures",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assistances_Asignatures_AsignatureId",
                table: "Assistances",
                column: "AsignatureId",
                principalTable: "Asignatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedules_Asignatures_AsignatureId",
                table: "Schedules",
                column: "AsignatureId",
                principalTable: "Asignatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
