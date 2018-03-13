using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;

public partial class _Default : System.Web.UI.Page
{
    //propriété qui représente notre classe de connexion et qui offre une passerelle vers la vraie connexion avec le pilote
    ConnexionBD bd = null;
    //propriété qui représente l'accès aux données (CRUD) pour un client
    Modele modeleClient = null;
    string codeJeu = null;
    /// <summary>
    /// Initialisation, première étape du cycle de vie donc on en profite pour créer notre modèle seulement s'il n'existe pas dans la session
    /// Après quoi, on ne le referra pas inutilement
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Init(object sender, EventArgs e)
    {
        //On ouvre pas une connexion si le modèle est déjà construit !
        //Il y a une nuance avec le postback ici car si on a frappé un problème dans le code, on aura mis le modèle à null dans la session
        //Ceci nous laisse donc une chance de le reconstruire !
        if (Session["modeleClient"] == null)
        {
            //Gestion des exception essentielle, on gère du code "dangereux"
            try
            {
                //On effectue la connexion et on l'obtient en retour
                bd = new ConnexionBD();
                OleDbConnection connection = bd.ConnectToDB(sqlDataSource1.ConnectionString);

                //On Instancie notre modèle client en lui passant la connection reçue
                modeleClient = new Modele(connection);

                //Si tout a fonctionné, on stocke notre modèle dans la session. C'est la meilleure façon car un PostBack va tout effacer ce qu'on vient de faire !
                Session["modeleClient"] = modeleClient;
                Session["CodeJeu"] = codeJeu;
            }

            catch (Exception exc)
            {
                //message d'erreur comme quoi la BD ne sera pas disponible
                modeleClient.RollbackTransaction();
                System.Diagnostics.Debug.Write(exc);
            }
        }
    }

    /// <summary>
    /// Chargement de la page, il faut faire attention au PostBack pour ne pas faire des requêtes inutilement à chaque PostBack
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {      
        //Pas besoin de lister les clients à chaque postback !
        if (!IsPostBack)
        {
            ListerJeux();
            ListerGenre(); 
            ListerNouveautes();
        }
    }

    protected void Page_Unload(object sender, EventArgs e)
    {
        //!!!!!!!!!!!!!!!!!!!!!!!!
        //CECI EST ESSENTIEL LORSQU'ON A FAIT DES REQUETES EN MODIFICATION CAR ELLES ÉTAIENT EN MEMOIRE DANS LA TRANSACTION, IL FAUT LES ÉCRIRE DANS LA BD
        //!!!!!!!!!!!!!!!!!!!!!!!!
        try
        {
            if (Session["modeleClient"] != null)
            {
                ((Modele)Session["modeleClient"]).CommitChanges();
            }
        }

        catch (Exception exc)
        {
            System.Diagnostics.Debug.Write(exc);
        }
    }

    //Fonction qui liste les jeux de la base de données.
    public void ListerJeux()
    {
        //Gestion des exceptions essentielle, on gère du code "dangereux"
        try
        {
            //On s'assure que le modèle client est disponible
            if (Session["modeleClient"] != null)
            {
                //On le récupère et on demande les enregistrements des clients selon la requête passée en paramètre
                Modele modele = (Modele)Session["modeleClient"];



                OleDbDataReader readerSelect = modele.ReadClient("SELECT Jeu.CodeJeu, Jeu.Titre, Jeu.Prix, Jeu.Plateforme, TypeJeu.Genre, Jeu.Description, Jeu.Image FROM Jeu INNER JOIN TypeJeu ON Jeu.IdGenre = TypeJeu.IdGenre");
                //OleDbDataReader readerSelectNom = modele.ReadClient("SELECT Nom FROM CLIENT");
                
                //On envoie notre table en construction
                  ConstruireTable(TableClient, readerSelect);
                  
                  
                    //!!!!!!!!!!!!!!!!!!!!!!!!
                    //CECI EST ESSENTIEL AVANT DE FAIRE UNE AUTRE REQUETE, CECI PERMETTRA UNE AUTRE
                    //REQUÊTE SUR LA COMMANDE QUI A ÉTÉ OUVERTE DANS LE MODÈLE. MÊME SI C'ÉTAIT LA DERNIÈRE REQUÊTE DU LOT IL FAUT LE FAIRE !!!
                    //!!!!!!!!!!!!!!!!!!!!!!!!
                    readerSelect.Close();
            }
        }
        catch (Exception exc)
        {
            //Si le probleme provenait de la transaction du modele, on la Rollback
            Modele modele = (Modele)Session["modeleClient"];
            modele.RollbackTransaction();
            //et on invalide le modele, il pourra être reconstruit en PostBack
            Session["modeleClient"] = null;
            System.Diagnostics.Debug.Write(exc);
        }    
    }

    //Fonction qui liste les genres de la base de données.
    public void ListerGenre()
    {
        //Gestion des exceptions essentielle, on gère du code "dangereux"
        try
        {
            //On s'assure que le modèle client est disponible
            if (Session["modeleClient"] != null)
            {
                //On le récupère et on demande les enregistrements des clients selon la requête passée en paramètre
                Modele modele = (Modele)Session["modeleClient"];



                OleDbDataReader readerSelect = modele.ReadClient("SELECT * FROM TypeJeu");

                //On place les éléments dans la liste déroulante.
                while (readerSelect.Read())
                {
                    DropDownListGenre.Items.Add(new ListItem(readerSelect[1].ToString()));        //(text,value)  

                }
                //!!!!!!!!!!!!!!!!!!!!!!!!
                //CECI EST ESSENTIEL AVANT DE FAIRE UNE AUTRE REQUETE, CECI PERMETTRA UNE AUTRE
                //REQUÊTE SUR LA COMMANDE QUI A ÉTÉ OUVERTE DANS LE MODÈLE. MÊME SI C'ÉTAIT LA DERNIÈRE REQUÊTE DU LOT IL FAUT LE FAIRE !!!
                //!!!!!!!!!!!!!!!!!!!!!!!!
                readerSelect.Close();
            }
        }
        catch (Exception exc)
        {
            //Si le probleme provenait de la transaction du modele, on la Rollback
            Modele modele = (Modele)Session["modeleClient"];
            modele.RollbackTransaction();
            //et on invalide le modele, il pourra être reconstruit en PostBack
            Session["modeleClient"] = null;
            System.Diagnostics.Debug.Write(exc);
        }
    }

    //Fonction qui liste les nouveautés de la base de données.
    public void ListerNouveautes()
    {
        //Gestion des exceptions essentielle, on gère du code "dangereux"
        try
        {
            //On s'assure que le modèle client est disponible
            if (Session["modeleClient"] != null)
            {
                //On le récupère et on demande les enregistrements des clients selon la requête passée en paramètre
                Modele modele = (Modele)Session["modeleClient"];



                OleDbDataReader readerSelect = modele.ReadClient("SELECT Jeu.CodeJeu, Jeu.Titre, Jeu.Prix, Jeu.Plateforme, TypeJeu.Genre, Jeu.Description, Jeu.Image FROM Jeu INNER JOIN TypeJeu ON Jeu.IdGenre = TypeJeu.IdGenre WHERE Jeu.Nouveaute = TRUE");

                //On place les éléments dans la boite-liste.
                while (readerSelect.Read())
                {
                    ListBoxNouveautes.Items.Add(new ListItem(readerSelect[1].ToString() + " " + readerSelect[2].ToString() + " " + readerSelect[3].ToString() + " " + readerSelect[4].ToString(), readerSelect[0].ToString()));        //(text,value)  

                }
                //!!!!!!!!!!!!!!!!!!!!!!!!
                //CECI EST ESSENTIEL AVANT DE FAIRE UNE AUTRE REQUETE, CECI PERMETTRA UNE AUTRE
                //REQUÊTE SUR LA COMMANDE QUI A ÉTÉ OUVERTE DANS LE MODÈLE. MÊME SI C'ÉTAIT LA DERNIÈRE REQUÊTE DU LOT IL FAUT LE FAIRE !!!
                //!!!!!!!!!!!!!!!!!!!!!!!!
                readerSelect.Close();
            }
        }
        catch (Exception exc)
        {
            //Si le probleme provenait de la transaction du modele, on la Rollback
            Modele modele = (Modele)Session["modeleClient"];
            modele.RollbackTransaction();
            //et on invalide le modele, il pourra être reconstruit en PostBack
            Session["modeleClient"] = null;
            System.Diagnostics.Debug.Write(exc);
        }
    }


    //Fonction qui construit le tableau contenant les jeux de la base de données.
    public void ConstruireTable(Table table, OleDbDataReader reader)
    {
        TableRow headerRow = new TableRow();
        TableHeaderCell cell = null;

        //extraction des champs pour créer l'entete
        for (int champ = 1; champ < reader.FieldCount; champ++)
        {
            cell = new TableHeaderCell();
            //On récupère le nom des champs ici
            cell.Text = reader.GetName(champ);
            //et ils deviennent le texte de nos entêtes
            headerRow.Cells.Add(cell);
        }

        //ajout d'une colonne qui ne vient pas de la base de données !
        cell = new TableHeaderCell();
        cell.Text = "detail";
        headerRow.Cells.Add(cell);

        //On ajoute la ligne des entêtes dans la table
        table.Rows.Add(headerRow);

        //extraction des enregistrements, on boucle le reader
        while (reader.Read())
        {
            TableRow row = new TableRow();
            TableCell tableCell = null;

            //Attention, on ne se rendra pas jusqu'au bout car la dernière entete ne provient pas de la BD !!!
            int numColumns = headerRow.Cells.Count - 1;

            //on se sert des noms de nos entetes comme clé pour récupérer nos enregistrements, encore une fois en ignorant la dernière qui a été créée manuellement !
            for (int i = 0; i < numColumns; i++)
            {
                tableCell = new TableCell();
                //on se sert du nom de l'entête ici comme clé
                tableCell.Text = reader[headerRow.Cells[i].Text].ToString();
                row.Cells.Add(tableCell);
               
                
            }
             
            if(cell.Text == "ps4.jpg")
            {
               Image myImage = new Image();
               myImage.ImageUrl = "ps4.png";
            }
            //On popule la dernière colonne en créant un hyperlien qui enverra le id en GET
            HyperLink link = new HyperLink();
            //La valeur du id se trouve dans la première cellule de la rangée courante, d'où le [0] pour aller le chercher
            link.NavigateUrl = "DetailJeu.aspx?codeJeu=" + reader[0].ToString();
            link.Text = "Voir les détails";


            

            //L'hyperlien étant un contrôle, on l'ajoute dans la cellule
            tableCell = new TableCell();
            tableCell.Controls.Add(link);
            row.Cells.Add(tableCell);

            //Et on ajoute notre ligne dans la table, on recommencera ce traitement pour tous les enregistrements
            table.Rows.Add(row);


        }
        
    }

    //Si un élément de la liste déroulante est changé.
    protected void DropDownListGenre_SelectedIndexChanged(object sender, EventArgs e)
    {
        string select = DropDownListGenre.SelectedItem.Value;


        if (Session["modeleClient"] != null)
        {
            //On le récupère et on demande les enregistrements des clients selon la requête passée en paramètre
            Modele modele = (Modele)Session["modeleClient"];
            OleDbDataReader readerSelect = null;

            if(select == "0")
            {
                readerSelect = modele.ReadClient("SELECT Jeu.CodeJeu, Jeu.Titre, Jeu.Prix, Jeu.Plateforme, TypeJeu.Genre, Jeu.Description, Jeu.Image FROM Jeu INNER JOIN TypeJeu ON Jeu.IdGenre = TypeJeu.IdGenre ORDER BY Jeu.Titre ASC");
            }
            else
            {
                readerSelect = modele.ReadClient("SELECT Jeu.CodeJeu, Jeu.Titre, Jeu.Prix, Jeu.Plateforme, TypeJeu.Genre, Jeu.Description, Jeu.Image FROM Jeu INNER JOIN TypeJeu ON Jeu.IdGenre = TypeJeu.IdGenre WHERE TypeJeu.Genre ='" + select + "'");
            }
            
            //OleDbDataReader readerSelectNom = modele.ReadClient("SELECT Nom FROM CLIENT");
            string iD = readerSelect.GetName(0);
            //On envoie notre table en construction
            ConstruireTable(TableClient, readerSelect);



            //while (readerSelect.Read())
            //{
            //    if (select == readerSelect[iD].ToString())
            //    {
                   
            //    }

            //    //UpdateCLient ("Select + nom + from")
            //}


            readerSelect.Close();
        }
    }
}