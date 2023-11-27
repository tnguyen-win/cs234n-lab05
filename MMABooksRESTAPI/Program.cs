using MMABooksEFClasses.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add CORS policy - in a production app lock this down!
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(
        builder => {
            builder
            .AllowAnyOrigin()
            .WithMethods("POST", "PUT", "DELETE", "GET", "OPTIONS")
            .AllowAnyHeader();
        });
});
// Adding the dbContext to the service
builder.Services.AddDbContext<MMABooksContext>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

// Enables the CORS policy
app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
