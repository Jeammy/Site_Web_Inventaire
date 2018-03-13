<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PointsDeDistribution.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:SqlDataSource ID="sqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:AppConnectionString %>"></asp:SqlDataSource>
    <br />
    <asp:Label ID="LabelTitrePage" runat="server" Text="Découvrez un point de distribution près de chez vous!" Font-Size="Larger"></asp:Label>
    <br />
    <br />
    <br />
    <%--<sspeichert>--%>
    <%--Voici les contrôles pour visualiser les points de distribution.--%>
    <asp:DropDownList ID="DropDownListVilles" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownListVilles_SelectedIndexChanged">
        <asp:ListItem Value="-1">Villes ayant un point de distribution</asp:ListItem>
    </asp:DropDownList>
    <br />
    <br />
    <asp:Label ID="LabelAdresse" runat="server" Text="Adresse :"></asp:Label>
    <br />
    <asp:Label ID="LabelVille" runat="server" Text="Ville :"></asp:Label>
    <br />
    <asp:Label ID="LabelPays" runat="server" Text="Pays :"></asp:Label>
    <br />
    <br />
    <br />
    <asp:Literal ID="LiteralMap" runat="server"></asp:Literal>

    <br />
    <br />
    <br />



<!--</sspeichert>-->  
</asp:Content>

