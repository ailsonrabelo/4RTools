using System.Windows.Forms;
using System;
using System.Windows.Input;
using _4RTools.Model;
using _4RTools.Utils;

namespace _4RTools.Forms
{
    public partial class AutopotForm : Form, IObserver
    {
        private Autopot autopot;
        private bool isYgg;

        // --- NOVOS ELEMENTOS VISUAIS PARA A CURA POR SKILL ---
        private Label lblHealSkill;
        private TextBox txtHealSkillKey;
        private TextBox txtHealSkillPct;

        public AutopotForm(Subject subject, bool isYgg)
        {
            InitializeComponent();
            if (isYgg)
            {
                this.picBoxHP.Image = Resources._4RTools.ETCResource.Yggdrasil;
                this.picBoxSP.Image = Resources._4RTools.ETCResource.Yggdrasil;
            }
            subject.Attach(this);
            this.isYgg = isYgg;

            // Inicializa e posiciona os campos de skill na tela dinamicamente
            InitializeHealSkillComponents();
        }

        private void InitializeHealSkillComponents()
        {
            // Criação do Label explicativo
            this.lblHealSkill = new Label();
            this.lblHealSkill.Text = "Skill Cura (Tecla / %):";
            this.lblHealSkill.Location = new System.Drawing.Point(12, 145); // Posição abaixo dos campos de SP/HP
            this.lblHealSkill.AutoSize = true;
            this.Controls.Add(this.lblHealSkill);

            // Campo de Texto para a Tecla da Skill
            this.txtHealSkillKey = new TextBox();
            this.txtHealSkillKey.Location = new System.Drawing.Point(135, 142);
            this.txtHealSkillKey.Width = 50;
            this.txtHealSkillKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            this.txtHealSkillKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            this.txtHealSkillKey.TextChanged += new EventHandler(this.onHealSkillKeyTextChange);
            this.Controls.Add(this.txtHealSkillKey);

            // Campo de Texto para a Porcentagem de HP da Skill
            this.txtHealSkillPct = new TextBox();
            this.txtHealSkillPct.Location = new System.Drawing.Point(195, 142);
            this.txtHealSkillPct.Width = 40;
            this.txtHealSkillPct.TextChanged += new EventHandler(this.onHealSkillPctTextChange);
            this.Controls.Add(this.txtHealSkillPct);
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.code)
            {
                case MessageCode.PROFILE_CHANGED:
                    this.autopot = this.isYgg ? ProfileSingleton.GetCurrent().AutopotYgg : ProfileSingleton.GetCurrent().Autopot;
                    InitializeApplicationForm();
                    break;
                case MessageCode.TURN_OFF:
                    this.autopot.Stop();
                    break;
                case MessageCode.TURN_ON:
                    this.autopot.Start();
                    break;
            }
        }

        private void InitializeApplicationForm()
        {
            this.txtHpKey.Text = this.autopot.hpKey.ToString();
            this.txtSPKey.Text = this.autopot.spKey.ToString();
            this.txtHPpct.Text = this.autopot.hpPercent.ToString();
            this.txtSPpct.Text = this.autopot.spPercent.ToString();
            this.txtAutopotDelay.Text = this.autopot.delay.ToString();

            // Carrega os valores salvos da skill de cura, se existirem
            if (this.txtHealSkillKey != null && this.autopot.healSkillKey != Key.None)
                this.txtHealSkillKey.Text = this.autopot.healSkillKey.ToString();

            if (this.txtHealSkillPct != null)
                this.txtHealSkillPct.Text = this.autopot.healSkillPercent.ToString();

            txtHpKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            txtHpKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            txtHpKey.TextChanged += new EventHandler(this.onHpTextChange);
            txtSPKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            txtSPKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            txtSPKey.TextChanged += new EventHandler(this.onSpTextChange);
        }

        private void onHpTextChange(object sender, EventArgs e)
        {
            try
            {
                Key key = (Key)Enum.Parse(typeof(Key), txtHpKey.Text.ToString());
                this.autopot.hpKey = key;
                ProfileSingleton.SetConfiguration(this.autopot);
            }
            catch (Exception) { }
        }

        private void onSpTextChange(object sender, EventArgs e)
        {
            try
            {
                Key key = (Key)Enum.Parse(typeof(Key), txtSPKey.Text.ToString());
                this.autopot.spKey = key;
                ProfileSingleton.SetConfiguration(this.autopot);
            }
            catch (Exception) { }
        }

        // --- EVENTOS PARA SALVAR OS DADOS DA CURA POR SKILL ---
        private void onHealSkillKeyTextChange(object sender, EventArgs e)
        {
            try
            {
                Key key = (Key)Enum.Parse(typeof(Key), txtHealSkillKey.Text.ToString());
                this.autopot.healSkillKey = key;
                ProfileSingleton.SetConfiguration(this.autopot);
            }
            catch (Exception)
            {
                this.autopot.healSkillKey = Key.None;
            }
        }

        private void onHealSkillPctTextChange(object sender, EventArgs e)
        {
            try
            {
                this.autopot.healSkillPercent = Int16.Parse(this.txtHealSkillPct.Text);
                ProfileSingleton.SetConfiguration(this.autopot);
            }
            catch (Exception)
            {
                this.autopot.healSkillPercent = 0;
            }
        }

        private void txtAutopotDelayTextChanged(object sender, EventArgs e)
        {
            try
            {
                this.autopot.delay = Int16.Parse(this.txtAutopotDelay.Text);
                ProfileSingleton.SetConfiguration(this.autopot);
            }
            catch (Exception) { }
        }

        private void txtHPpctTextChanged(object sender, EventArgs e)
        {
            try
            {
                this.autopot.hpPercent = Int16.Parse(this.txtHPpct.Text);
                ProfileSingleton.SetConfiguration(this.autopot);
            }
            catch (Exception) { }

        }

        private void txtSPpctTextChanged(object sender, EventArgs e)
        {
            try
            {
                this.autopot.spPercent = Int16.Parse(this.txtSPpct.Text);
                ProfileSingleton.SetConfiguration(this.autopot);
            }
            catch (Exception) { }
        }
    }
}
