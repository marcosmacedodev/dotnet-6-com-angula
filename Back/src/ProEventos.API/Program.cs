using Microsoft.EntityFrameworkCore;
using ProEventos.Application.Contracts;
using ProEventos.Application.Services;
using ProEventos.Persistence.Contexts;
using ProEventos.Persistence.Contracts;
using ProEventos.Persistence.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ProEventosContext>(context => context.UseInMemoryDatabase("EventoList"));
builder.Services.AddControllers()
.AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<ILoteService, LoteService>();

builder.Services.AddScoped<IRepository, RepositoryImpl>();
builder.Services.AddScoped<IRepositoryEvento, RepositoryEventoImpl>();
builder.Services.AddScoped<IRepositoryLote, RepositoryLoteImpl>();


builder.Services.AddCors();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseExceptionHandler("/error");

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.MapControllers();

app.Run();
