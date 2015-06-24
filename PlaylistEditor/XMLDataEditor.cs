using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;

namespace PlaylistEditor
{
    public partial class XMLDataEditor : Form
    {

        private XmlSchema schema = null;
        private XmlDataDocument doc = null;

        public XMLDataEditor()
        {
            InitializeComponent();
        }

        private void XMLDataEditor_Load(object sender, EventArgs e)
        {

        }

        private void btnOpenXML_Click(object sender, EventArgs e)
        {
            schema = InferXMLSchema("gamelist.xml");
 
            doc = new XmlDataDocument();
            doc.DataSet.ReadXml("gamelist.xml");
 
            ConstructGUI(doc.DataSet);
        }
 
        private XmlSchema InferXMLSchema(string filename)
        {
            XmlSchema infSchema = null;
            XmlReader reader = XmlReader.Create(filename);
 
            XmlSchemaSet schemaSet = new XmlSchemaSet();
            XmlSchemaInference inference = new XmlSchemaInference();
 
            schemaSet = inference.InferSchema(reader);
 
            foreach (XmlSchema sch in schemaSet.Schemas())
            {
                infSchema = sch;
            }
            return infSchema;
        }
 
        private void ConstructGUI(DataSet dataSet)
        {
            Point pos = new Point(10, 10);
 
            foreach (DataTable dt in dataSet.Tables)
            {
                if (dt.ParentRelations.Count == 0)
                {
                    XmlSchemaElement el = GetGlobalElement(dataSet.DataSetName);
                    XmlSchemaComplexType ct = (XmlSchemaComplexType)el.ElementSchemaType;
 
                    pnlDynForm.Controls.Clear();
                    Point p2 = ConstructGUI(pos.X, pos.Y, dt, pnlDynForm, ct);
                    pos.Y += p2.Y;
                }
            }
        }
 
        private Point ConstructGUI(int absx, int absy, DataTable dt, Control gbParent, XmlSchemaComplexType ct)
        {
            try
            {
                int rowcount = 0;
                foreach(DataRow row in dt.Rows)
                {
                    if (rowcount < 100)
                    {
                        int rely = absy;
                        foreach (Control ctrl in gbParent.Controls) { rely += (ctrl.Height + 5); }

                        GroupBox gb1 = new GroupBox();
                        gb1.Font = new Font(gb1.Font, FontStyle.Bold);
                        gb1.Text = dt.TableName;
                        gb1.Location = new Point(absx, rely);
                        gb1.Parent = gbParent;
                        gb1.Visible = true;
                        gb1.AutoSize = true;

                        int rowcols = 0;
                        // For each column in the table...
                        foreach (DataColumn col in dt.Columns)
                        {
                            int ctrly = 5;
                            foreach (Label ctrl in gb1.Controls.OfType<Label>()) { ctrly += ctrl.Height; }
                            // if it's not an internal ID...
                            if (col.ColumnMapping != MappingType.Hidden)
                            {
                                Label lbl = CreateLabel(10, ctrly + 10, col.ColumnName, gb1);
                                CreateTextBox(lbl.Width + 10, ctrly + 10, row[rowcols].ToString().Trim(), gb1);
                                rowcols++;
                            }
                        }
                        ++rowcount;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.ErrorRoutine(false, ex);
            }
            return new Point();
        }
 
        private Label CreateLabel(int absx, int absy, string txt, Control ctrlParent)
        {
            try
            {
                Label lb = new Label();
                lb.Font = new Font(lb.Font, FontStyle.Regular);
                lb.Text = txt + ":";
                lb.Location = new Point(absx, absy);
                lb.Parent = ctrlParent;
                lb.Width = 75;
                lb.Visible = true;

                return lb;
            }
            catch (Exception ex)
            {
                ErrorHandler.ErrorRoutine(false, ex);
                return new Label();
            }
        }
 
        private TextBox CreateTextBox(int absx, int absy, string txt, Control ctrlParent)
        {
            try
            {
                TextBox tb = new TextBox();
                tb.Font = new Font(tb.Font, FontStyle.Regular);
                tb.Text = txt;
                tb.Location = new Point(absx, absy);
                tb.Parent = ctrlParent;
                tb.Margin = new Padding(3);
                tb.Width = 300;
                tb.Visible = true;


                return tb;
            }
            catch (Exception ex)
            {
                ErrorHandler.ErrorRoutine(false, ex);
                return new TextBox();
            }
        }
 
        private XmlSchemaElement GetGlobalElement(string name)
        {
            XmlQualifiedName qname = new XmlQualifiedName(name, schema.TargetNamespace);
            XmlSchemaObject obj = schema.Elements[qname];
            return (XmlSchemaElement)obj;
        }
 
        private XmlSchemaComplexType GetGlobalComplexType(string name)
        {
            for (int i = 0; i < schema.Items.Count; i++)
            {
                XmlSchemaComplexType obj = schema.Items[i] as XmlSchemaComplexType;
                if (obj != null)
                {
                    if (obj.Name == name)
                    {
                        return obj;
                    }
                }
            }
            return null;
        }
    }
}