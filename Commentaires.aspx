<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Commentaires.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:SqlDataSource ID="sqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:AppConnectionString %>"></asp:SqlDataSource>
    <asp:Label ID="LabelTitrePage" runat="server" Text="N'hésitez pas à nous faire part de vos commentaires!" Font-Size="Larger"></asp:Label>
    <br />
    <br />
    <br />
<!--<sspeichert>-->     
    <asp:Label ID="LabelValidation" runat="server" Font-Bold="True" ForeColor="#006600"></asp:Label>
    
    <br />
    <br />
    <asp:Label ID="LabelPrenom" runat="server" Text="Prenom :"></asp:Label>
    <asp:TextBox ID="TextBoxPrenom" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TextBoxPrenom" EnableClientScript="False" ErrorMessage="RequiredFieldValidator" Font-Bold="True" ForeColor="#990000">Le prénom est inexistant.</asp:RequiredFieldValidator>
    <br />
    <asp:Label ID="LabelNom" runat="server" Text="Nom :"></asp:Label>
    <asp:TextBox ID="TextBoxNom" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TextBoxNom" EnableClientScript="False" ErrorMessage="RequiredFieldValidator" Font-Bold="True" ForeColor="#990000">Le nom est inexistant.</asp:RequiredFieldValidator>
    <br />
    <asp:Label ID="LabelCommentaire" runat="server" Text="Commentaire (100 caractères max) :"></asp:Label>
    <br />
    <asp:TextBox ID="TextBoxCommentaire" runat="server" Height="117px" MaxLength="100" Width="200px" TextMode="MultiLine"></asp:TextBox>
    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="TextBoxCommentaire" EnableClientScript="False" ErrorMessage="RequiredFieldValidator" Font-Bold="True" ForeColor="#990000">Le commentaire est inexistant.</asp:RequiredFieldValidator>
    <br />
    <br />
    <asp:Button ID="ButtonEnvoyer" runat="server" OnClick="ButtonEnvoyer_Click" Text="Envoyer le commentaire" />
    <br />
    
    <br />
    <br />
    <br />


<!--</sspeichert>-->  

</asp:Content>

