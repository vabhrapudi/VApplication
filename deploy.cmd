@if "%SCM_TRACE_LEVEL%" NEQ "4" @echo off

IF "%SITE_ROLE%" == "bot" (
 
  deploy.bot.cmd
 
) ELSE (
 
    deploy.function.cmd

)