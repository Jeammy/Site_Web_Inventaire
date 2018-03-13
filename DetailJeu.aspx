<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DetailJeu.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:SqlDataSource ID="sqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:AppConnectionString %>"></asp:SqlDataSource>
    <asp:Label ID="LabelTitrePage" runat="server" Text="Détails d'un jeu!" Font-Size="Larger"></asp:Label>
    <br />
    <br />
    <br />
<!--<sspeichert>--> 

    <link href="CSS/styles.css" rel="stylesheet" />

    <asp:Image ID="ImageJeu" runat="server" Height="307px" Width="582px" ImageUrl="~/img/miniatures/nes.png" />



    <%--Labels contenant les informations du jeu sélectionné.--%>
    <br />
    <br />
    <br />
    <asp:Label ID="LabelTitreDuJeu" runat="server" Text="Titre du jeu :"></asp:Label>
    <br />
    <asp:Label ID="LabelGenre" runat="server" Text="Genre :"></asp:Label>
    <br />
    <asp:Label ID="LabelPlateforme" runat="server" Text="Plateforme :"></asp:Label>
    <br />
    <asp:Label ID="LabelPrix" runat="server" Text="Prix :"></asp:Label>
    <br />
    <asp:Label ID="LabelDeveloppeur" runat="server" Text="Développeur :"></asp:Label>
    <br />
    <asp:Label ID="LabelSynopsis" runat="server" Text="Synopsis :"></asp:Label>
    <br />
    <br />
    <br />
    <br />
    <%--Hyperlien pour revenir à la page d'administration.--%>
    <asp:HyperLink ID="HyperLinkBackToMainPage" runat="server" NavigateUrl="~/Default.aspx">Retourner à la page d'accueil</asp:HyperLink>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />



<!--</sspeichert>-->  
</asp:Content>

