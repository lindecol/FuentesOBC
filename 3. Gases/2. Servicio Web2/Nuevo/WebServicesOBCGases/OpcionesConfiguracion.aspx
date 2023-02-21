<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OpcionesConfiguracion.aspx.cs" Inherits="WebServicesOBCGases.OpcionesConfiguracion" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml" >
	<head>
		<title>Default</title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR"/>
		<meta content="C#" name="CODE_LANGUAGE"/>
		<meta content="JavaScript" name="vs_defaultClientScript"/>
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema"/>
	</head>
	<body >
		<form id="Form1" method="post" runat="server">
			<asp:panel id="pnInicio" style="Z-INDEX: 113; LEFT: 8px; POSITION: absolute; TOP: 8px" runat="server"
				BackImageUrl="~/Imagenes/Vanner.bmp" Height="455px" Width="846px"></asp:panel><asp:panel id="pnConexionBaseDatos" style="Z-INDEX: 114; LEFT: 76px; POSITION: absolute; TOP: 109px"
				runat="server" Height="321px" Width="552px" BorderWidth="1px" BorderStyle="Solid" HorizontalAlign="Center" BorderColor="OliveDrab" BackColor="White">
				<table id="Table4" style="WIDTH: 542px; HEIGHT: 56px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; border-bottom: white thin solid;" cellspacing="1"
					cellpadding="1" width="542" border="0">
					<tr>
						<td style="HEIGHT: 20px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; border-bottom: white thin solid;" align="center"></td>
						<td style="HEIGHT: 20px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; border-bottom: white thin solid;" align="center"></td>
						<td style="HEIGHT: 20px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; border-bottom: white thin solid;" align="center"></td>
					</tr>
					<tr>
						<td style="HEIGHT: 20px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; border-bottom: white thin solid;" align="center"></td>
						<td style="HEIGHT: 20px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; border-bottom: white thin solid;" align="center">
							<asp:Image id="imgConexion" runat="server" Width="53px" Height="49px" ImageUrl="Imagenes/Llaves.bmp"
								ImageAlign="Middle"></asp:Image>
							<asp:label id="Label30" runat="server" Width="363px" Height="16px" ForeColor="DarkGreen" Font-Names="Verdana"
								Font-Bold="True" Font-Size="Smaller">Datos Conexion Servidor ORACLE</asp:label></td>
						<td style="HEIGHT: 20px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; border-bottom: white thin solid;" align="center"></td>
					</tr>
				</table>
				<table id="Table6" style="WIDTH: 506px; HEIGHT: 56px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; border-bottom: white thin solid;" cellspacing="1"
					cellpadding="1" width="506" border="1">
					<tr>
						<td style="WIDTH: 144px; HEIGHT: 6px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; border-bottom: white thin solid;"></td>
						<td style="HEIGHT: 6px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; width: 144px; border-bottom: white thin solid;"></td>
						<td style="HEIGHT: 6px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; width: 144px; border-bottom: white thin solid;"></td>
					</tr>
					<tr>
						<td style="WIDTH: 144px; HEIGHT: 6px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; border-bottom: white thin solid;">
							<asp:label id="lblDSN" runat="server" Width="122px" Height="18px" ForeColor="DarkGreen" Font-Names="Verdana"
								Font-Bold="True" Font-Size="Smaller">DSN</asp:label></td>
						<td style="HEIGHT: 6px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; width: 144px; border-bottom: white thin solid;">
							<asp:TextBox id="txtDSN" runat="server" Width="178px" Height="24px" Font-Names="Verdana" Font-Size="Smaller"></asp:TextBox></td>
						<td style="HEIGHT: 6px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; width: 144px; border-bottom: white thin solid;"></td>
					</tr>
					<tr>
						<td style="WIDTH: 144px; HEIGHT: 6px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; border-bottom: white thin solid;">
							<asp:label id="lblUsuario" runat="server" Width="122px" Height="18px" ForeColor="DarkGreen"
								Font-Names="Verdana" Font-Bold="True" Font-Size="Smaller">Nombre Usuario:</asp:label></td>
						<td style="HEIGHT: 6px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; width: 144px; border-bottom: white thin solid;">
							<asp:TextBox id="txtUsuarioOracle" runat="server" Width="178px" Height="24px" Font-Names="Verdana"
								Font-Size="Smaller"></asp:TextBox></td>
						<td style="HEIGHT: 6px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; width: 144px; border-bottom: white thin solid;"></td>
					</tr>
					<tr>
						<td style="WIDTH: 144px; HEIGHT: 6px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; border-bottom: white thin solid;">
							<asp:label id="lblContraseña" runat="server" Width="138px" Height="18px" ForeColor="DarkGreen"
								Font-Names="Verdana" Font-Bold="True" Font-Size="Smaller">Contraseña:</asp:label></td>
						<td style="HEIGHT: 6px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; width: 144px; border-bottom: white thin solid;">
							<asp:TextBox id="txtContrasenaOracle" runat="server" Width="178px" Height="24px" Font-Names="Verdana"
								Font-Size="Smaller" TextMode="Password"></asp:TextBox></td>
						<td style="HEIGHT: 6px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; width: 144px; border-bottom: white thin solid;"></td>
					</tr>
					<tr>
						<td style="WIDTH: 144px; HEIGHT: 6px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; border-bottom: white thin solid;">
                            <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Verdana" Font-Size="Smaller"
                                ForeColor="DarkGreen" Height="18px" Width="138px">Codigo Programa:</asp:Label></td>
						<td style="HEIGHT: 6px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; width: 144px; border-bottom: white thin solid;">
                            <asp:TextBox ID="txtCodigoPrograma" runat="server" Font-Names="Verdana" Font-Size="Smaller"
                                Height="24px" Width="178px"></asp:TextBox></td>
						<td style="HEIGHT: 6px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; width: 144px; border-bottom: white thin solid;"></td>
					</tr>
					<tr>
						<td style="WIDTH: 144px; HEIGHT: 6px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; border-bottom: white thin solid; text-align: left;" ></td>
						<td style="HEIGHT: 6px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; width: 144px; border-bottom: white thin solid; text-align: left;" >
							<asp:button id="btnModificarOracle" runat="server" Width="178px" Height="24px" BackColor="Window"
								BorderColor="DarkGreen" Font-Names="Verdana" Font-Size="Smaller" Text="Modificar" OnClick="btnModificarOracle_Click"></asp:button></td>
						<td style="HEIGHT: 6px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; width: 144px; border-bottom: white thin solid; text-align: left;" >
							</td>
					</tr>
					<tr>
						<td style="WIDTH: 144px; HEIGHT: 25px; border-right: white thin solid; border-top: white thin solid; border-left: white thin solid; border-bottom: white thin solid;"  align="center" colspan="3"></td>
					</tr>
				</table>
				<asp:label id="lblAdvertenciaOracle" runat="server" Width="363px" Height="16px" ForeColor="LimeGreen"
					Font-Names="Verdana" Font-Bold="True" Font-Size="Smaller"></asp:label>
			</asp:panel></form>
	</body>
</html>
