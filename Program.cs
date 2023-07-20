var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "CookieAuthentication";
    options.DefaultSignInScheme = "CookieAuthentication";
    options.DefaultChallengeScheme = "CookieAuthentication";
})
.AddCookie("CookieAuthentication", options =>
{
    options.LoginPath = "/Conta/Login"; // Página de login
    options.AccessDeniedPath = "/Conta/AcessoNegado"; // Página de acesso negado (opcional)
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Conta}/{action=Login}/{id?}");
app.MapControllerRoute(
    name: "loginDireto", // Renomeado para "loginDireto"
    pattern: "Conta/Login/{email?}/{senha?}",
    defaults: new { controller = "Conta", action = "LoginDireto" } // Renomeado para "LoginDireto"
);
// Outras rotas aqui

app.Run();
