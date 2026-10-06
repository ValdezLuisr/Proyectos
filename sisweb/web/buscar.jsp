
<%@page contentType="text/html" pageEncoding="UTF-8"%>
<!DOCTYPE html>
<html>
    <%
        String metodo = request.getMethod();
        String accion = "";
        if ("GET".equalsIgnoreCase(metodo)) {
            response.sendRedirect("login.jsp");
            return;
        } else {
            accion = request.getParameter("accion");
        }
    %>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <title>JSP Page</title>
        <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-sRIl4kxILFvY47J16cr9ZwB07vP4J8+LH7qKQnuqkuIAvNWLzeN8tE5YBujZqJLB" crossorigin="anonymous">

    </head>
    <body>
        <h1>Hello World!</h1>
        <%
            out.print("El valor accion es: " + accion);
            String pagina = "";
            if("buscar".equalsIgnoreCase(accion) || "eliminar".equalsIgnoreCase(accion)){
                pagina = "confirmar.jsp";
            }
            else if("editar".equalsIgnoreCase(accion)){
                pagina = "registro.jsp";
            }
            
        %>
        <div class="container">
            <form action="<%=pagina%>" method="post">
                <button type="submit" class="btn btn-primary mb-3" name="accion" value="<%=accion%>">Buscar</button><br>
            </form>
        </div>
        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.8/dist/js/bootstrap.bundle.min.js" integrity="sha384-FKyoEForCGlyvwx9Hj09JcYn3nv7wiPVlz7YYwJrWVcXK/BmnVDxM+D2scQbITxI" crossorigin="anonymous"></script>

    </body>
</html>
