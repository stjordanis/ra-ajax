/*
 * Ra-Ajax - A Managed Ajax Library for ASP.NET and Mono
 * Copyright 2008 - 2009 - Thomas Hansen thomas@ra-ajax.org
 * This code is licensed under the GPL version 3 which 
 * can be found in the license.txt file on disc.
 *
 */

using System;
using System.ComponentModel;
using WEBCTRLS = System.Web.UI.WebControls;
using ASP = System.Web.UI;
using Ra.Widgets;
using System.IO;
using HTML = System.Web.UI.HtmlControls;
using System.Collections.Generic;

namespace Ra.Extensions.Widgets
{
    /**
     * Calendar widget for choosing dates. Basically just a DateTimePicker control without the Time portions
     * which makes it possible to choose dates. Use the DateTimePicker widget if you also need Time support.
     * This is a highly flexible control and you can even override the rendering of cells by handling the 
     * RenderDay event from which you can even inject your own controls into specific dates and such. Highly
     * versatile Control for date manipulations.
     */
    [ASP.ToolboxData("<{0}:Calendar runat=server />")]
    public class Calendar : Panel, ASP.INamingContainer
    {
        /**
         * EventArgs passed to the CreateFooterControls events. This is the EventArgs
         * passed to the CreateFooterControls event of the Calendar Widget. To add your 
         * own custom controls at the bottom append these controls into the Controls property 
         * of this EventArgs instance. A good example of using this can be seen in the DateTimePicker 
         * of Ra-Ajax which injects its TextBoxes for choosing the time parts using this logic.
         */
        public class CreateFooterControlsEventArgs : EventArgs
        {
            private List<ASP.Control> _ctrls = new List<ASP.Control>();

            internal CreateFooterControlsEventArgs()
            { }

            /**
             * Append your controls into this list and they will be automatically 
             * appended at the bottom of the control. Note that for every control
             * in this list you will get one new row in the table actually composing 
             * the Calendar. This means that if you want to have more then one control
             * per row in the table you must append those two controls into a Literal
             * control or something which wraps them both.
             */
            public List<ASP.Control> Controls
            {
                get { return _ctrls; }
            }
        }

        /**
         * EventArgs class for the RenderDay Event. Modify the Cell property to e.g. add up 
         * child controls or special styles and CSS classes and such. Useful for creating e.g. 
         * "activities" or something similar on specific dates according to your own entity 
         * DateTime objects.
         */
        public class RenderDayEventArgs : EventArgs
        {
            private DateTime _date;
            private HTML.HtmlTableCell _cell;

            /**
             * Cell which renders the given Date. This is an HTML table td element which you can inject your
             * own controls within. You can also modify the style or CssClass properties of this control
             * to reflect different visual appearance for different dates.
             */
            public HTML.HtmlTableCell Cell
            {
                get { return _cell; }
            }

            /**
             * Date which are currently rendered. This is the DateTime object for the cell currently being
             * rendered.
             */
            public DateTime Date
            {
                get { return _date; }
            }

            internal RenderDayEventArgs(DateTime date, HTML.HtmlTableCell cell)
            {
                _date = date;
                _cell = cell;
            }
        }

        Label _nw;
        Label _n;
        Label _ne;
        Label _w;
        Label _body;
        Label _content = new Label();
        Label _e;
        Label _sw;
        Label _s;
        Label _se;
        protected Label _caption;

        /**
         * Raised when Value is changed by user. Can be raised by changing month and year contrary
         * to the DateClicked event. This is NOT normally supposed to be thought of as the
         * "date chosen by user event" but rather the DateClicked event serves that purpose. The
         * reason to that is that the user might just browse around to find the "correct" month or year
         * etc. Not before the DateClicked event is raised one would normally consider the user to
         * have finished his selection of date.
         */
        public event EventHandler SelectedValueChanged;

        /**
         * Raised when a specific date is clicked, this event will not be raised when
         * year or month is changed, only when a specific date is clicked or the "Today" date
         * is clicked. This can be thought of as the "date was selected event".
         */
        public event EventHandler DateClicked;

        /**
         * Handle this event if you need to create extra controls at the bottom of
         * the calendar for any reasons. Ra-Ajax uses this one internally to create its
         * DateTimePicker control
         */
        public event EventHandler<CreateFooterControlsEventArgs> CreateFooterControls;

        /**
         * Called once for every date which is rendered within the current month if defined. Use this
         * event to render your own custom content within specific dates. The EventArgs of this
         * event handler is RenderDayEventArgs which has one property for what date is currently being 
         * rendered, and another property called Cell which is the currently rendered HTML table td 
         * cell of the control. Inside of this Cell control you can add up your own controls and such.
         * Or modify the CssClass and such to have different colors and such for different dates.
         * Notice though that this event will be raise once for EVERY date within the currently 
         * selected month. So be careful with adding too much overhead into your event handlers handling
         * this event.
         */
        public event EventHandler<RenderDayEventArgs> RenderDay;

        /**
         * Overridden to provide a sane default value. The default value of this property is "calendar".
         */
        [DefaultValue("calendar")]
        public override string CssClass
        {
            get
            {
                return string.IsNullOrEmpty(base.CssClass) ? "calendar" : base.CssClass;
            }
            set
            {
                bool sameClass = value == base.CssClass;
                base.CssClass = value;
                if (!sameClass && _nw != null)
                {
                    // If class has actually *changed* AND we have created the child composition
                    // controls...
                    SetCssClassesAndMore();
                }
            }
        }

        /**
         * Selected value of calendar. This is the currently selected DateTime value of the control.
         * You can also set this value which will be reflected back to the client once returned from
         * the Ajax Request.
         */
        public DateTime Value
        {
            get { return ViewState["Value"] == null ? DateTime.Now.Date : (DateTime)ViewState["Value"]; }
            set
            {
                DateTime oldValue = Value;
                ViewState["Value"] = value;
                if (oldValue.Date != value.Date)
                {
                    // For cases where only the time parts have changed we do NOT
                    // re-render the controls
                    _content.Controls.Clear();
                    CreateCalendarControls();
                    _content.ReRender();
                }
            }
        }

        /**
         * Defines the leftmost weekday of the calendar. In Norway for instance a week starts on Mondays, 
         * while in most other countries the week starts on Sunday. Change this property to define
         * which weekday you wish to have to the left on the Calendar surface. You can choose any weekday
         * you wish here.
         */
        [DefaultValue(DayOfWeek.Monday)]
        public DayOfWeek StartsOn
        {
            get { return ViewState["StartsOn"] == null ? DayOfWeek.Monday : (DayOfWeek)ViewState["StartsOn"]; }
            set
            {
                ViewState["StartsOn"] = value;
            }
        }

        /**
         * Overrides the caption of the Calendar. The caption will normally display the "current date" unless
         * this property is set to something else than null.
         */
        [DefaultValue(null)]
        public string Caption
        {
            get
            {
                if (ViewState["Caption"] == null)
                    return null;
                return (string)ViewState["Caption"];
            }
            set { ViewState["Caption"] = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Since we're dependant upon that the ViewState has finished loading before
            // we initialize the ChildControls since how the child controls (and which)
            // child controls are being re-created is dependant upon a ViewState saved value
            // this is the earliest possible time we can reload the ChildControls for the
            // Control
            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();
            _content.Controls.Clear();
            CreateWindowControls();
            CreateCalendarControls();
            Controls.Add(_nw);
            Controls.Add(_body);
        }

        private void CreateWindowControls()
        {
            // Gteting CssClass of Calendar control
            string cssClass = CssClass;
            if (cssClass.IndexOf(" ") != -1)
                cssClass = cssClass.Split(' ')[0];

            // Top parts
            _nw = new Label();
            _nw.Tag = "div";
            _nw.ID = "nw";
            _nw.CssClass = cssClass + "_nw";

            _ne = new Label();
            _ne.Tag = "div";
            _ne.ID = "ne";
            _ne.CssClass = cssClass + "_ne";
            _nw.Controls.Add(_ne);

            _n = new Label();
            _n.Tag = "div";
            _n.ID = "n";
            _n.CssClass = cssClass + "_n";
            _ne.Controls.Add(_n);

            _caption = new Label();
            _caption.ID = "capt";
            _caption.CssClass = cssClass + "_title";
            _n.Controls.Add(_caption);

            // Middle parts
            _body = new Label();
            _body.Tag = "div";
            _body.ID = "body";
            _body.CssClass = cssClass + "_body";

            _w = new Label();
            _w.Tag = "div";
            _w.ID = "w";
            _w.CssClass = cssClass + "_w";
            _body.Controls.Add(_w);

            _e = new Label();
            _e.Tag = "div";
            _e.ID = "e";
            _e.CssClass = cssClass + "_e";
            _w.Controls.Add(_e);

            _content.Tag = "div";
            _content.ID = "content";
            _content.CssClass = cssClass + "_content calendar";
            _e.Controls.Add(_content);

            // Bottom parts
            _sw = new Label();
            _sw.Tag = "div";
            _sw.ID = "sw";
            _sw.CssClass = cssClass + "_sw";
            _body.Controls.Add(_sw);

            _se = new Label();
            _se.Tag = "div";
            _se.ID = "se";
            _se.CssClass = cssClass + "_se";
            _sw.Controls.Add(_se);

            _s = new Label();
            _s.Tag = "div";
            _s.ID = "s";
            _s.Text = "&nbsp;";
            _s.CssClass = cssClass + "_s";
            _se.Controls.Add(_s);
        }

        private void CreateCalendarControls()
        {
            // Finding date to start on
            DateTime idxDate = FindStartDate();

            // Creating table to wrap the whole thing inside of
            HTML.HtmlTable tbl = new HTML.HtmlTable();
            tbl.EnableViewState = false;
            tbl.ID = "tbl_" + Value.ToString("dd_MM_yyyy", System.Globalization.CultureInfo.InvariantCulture);

            // Creating the header row which contains the Year and Month DropDownLists
            CreateYearMonthPicker(tbl);

            // Creating Weekdays at top of calendar
            idxDate = CreateWeekDaysAtTop(idxDate, tbl);

            // Looping through creating childcontrols
            while (true)
            {
                HTML.HtmlTableRow row = new HTML.HtmlTableRow();
                row.EnableViewState = false;
                row.ID = idxDate.ToString("MM_dd", System.Globalization.CultureInfo.InvariantCulture) + Value.ToString("dd_MM_yyyy", System.Globalization.CultureInfo.InvariantCulture);

                idxDate = CreateWeekNumberCell(idxDate, row);

                // Creating week cells for actual dates...
                for (int idx = 0; idx < 7; idx++)
                {
                    idxDate = CreateOneDayCell(idxDate, row);
                }
                tbl.Rows.Add(row);
                if (idxDate.Month != Value.Month)
                    break;
            }

            // Creating "extra controls" to append at bottom
            if (CreateFooterControls != null)
            {
                CreateFooterControlsEventArgs evt = new CreateFooterControlsEventArgs();
                CreateFooterControls(this, evt);
                int idxNo = 0;
                foreach (ASP.Control idx in evt.Controls)
                {
                    HTML.HtmlTableRow row = new HTML.HtmlTableRow();
                    row.ID = "extraRow" + idxNo;
                    HTML.HtmlTableCell cell = new HTML.HtmlTableCell();
                    cell.ID = "extraCell" + idxNo;
                    cell.ColSpan = 8;
                    cell.Controls.Add(idx);
                    row.Cells.Add(cell);
                    tbl.Rows.Add(row);
                    idxNo += 1;
                }
            }

            // Today button...
            CreateTodayButton(tbl);

            // Rooting the Table as the LAST thing we do...!
            _content.Controls.Add(tbl);
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (Caption == null)
                _caption.Text = Value.ToString("dddd - dd. MMM yy", System.Globalization.CultureInfo.InvariantCulture);
            else
                _caption.Text = Caption;

            // Calling base...
            base.OnPreRender(e);
        }

        private void SetCssClassesAndMore()
        {
            // Setting the CSS classes for all "decoration controls"
            string cssClass = CssClass;
            if (cssClass.IndexOf(" ") != -1)
                cssClass = cssClass.Split(' ')[0];
            _nw.CssClass = cssClass + "_nw";
            _n.CssClass = cssClass + "_n";
            _ne.CssClass = cssClass + "_ne";
            _e.CssClass = cssClass + "_e";
            _se.CssClass = cssClass + "_se";
            _s.CssClass = cssClass + "_s";
            _sw.CssClass = cssClass + "_sw";
            _w.CssClass = cssClass + "_w";
            _content.CssClass = cssClass + "_content calendar";
            _body.CssClass = cssClass + "_body";
            _caption.CssClass = cssClass + "_title";
        }

        private DateTime FindStartDate()
        {
            DateTime idxDate = new DateTime(Value.Year, Value.Month, 1);
            while (idxDate.DayOfWeek != StartsOn)
            {
                idxDate = idxDate.AddDays(-1);
            }
            return idxDate;
        }

        private void CreateTodayButton(HTML.HtmlTable tbl)
        {
            HTML.HtmlTableRow bottomRow = new HTML.HtmlTableRow();
            bottomRow.EnableViewState = false;
            bottomRow.ID = "stat_" + Value.ToString("dd_MM_yyyy", System.Globalization.CultureInfo.InvariantCulture);
            HTML.HtmlTableCell bottomCell = new HTML.HtmlTableCell();
            bottomCell.EnableViewState = false;
            bottomCell.Style["text-align"] = "center";
            bottomCell.ID = "bottC_" + Value.ToString("dd_MM_yyyy", System.Globalization.CultureInfo.InvariantCulture);
            bottomCell.ColSpan = 8;
            LinkButton today = new LinkButton();
            today.ID = "todayBtn";
            today.Text = "Today";
            today.Click += new EventHandler(today_Click);
            today.EnableViewState = false;
            bottomCell.Controls.Add(today);
            bottomRow.Cells.Add(bottomCell);
            tbl.Rows.Add(bottomRow);
        }

        private DateTime CreateOneDayCell(DateTime idxDate, HTML.HtmlTableRow row)
        {
            HTML.HtmlTableCell cell = new HTML.HtmlTableCell();
            cell.EnableViewState = false;
            cell.ID = idxDate.ToString("cell_MM_dd", System.Globalization.CultureInfo.InvariantCulture) + Value.ToString("dd_MM_yyyy", System.Globalization.CultureInfo.InvariantCulture);

            // Creating actual LinkButton which is the "clickable part" of the calendar days
            LinkButton btn = new LinkButton();
            btn.Text = idxDate.Day.ToString();
            btn.ID = idxDate.ToString("yyyy_MM_dd", System.Globalization.CultureInfo.InvariantCulture);
            if (idxDate == Value.Date)
            {
                btn.CssClass = "selected";
            }
            else if (idxDate.Month != Value.Month)
                btn.CssClass = "offMonth";
            btn.Click += new EventHandler(btn_Click);
            btn.EnableViewState = false;
            cell.Controls.Add(btn);

            if (RenderDay != null)
                RenderDay(this, new RenderDayEventArgs(idxDate.Date, cell));

            row.Cells.Add(cell);

            idxDate = idxDate.AddDays(1);
            return idxDate;
        }

        private DateTime CreateWeekNumberCell(DateTime idxDate, HTML.HtmlTableRow row)
        {
            // Creating week number cell
            // Calculates according to ISO-8601
            HTML.HtmlTableCell week = new HTML.HtmlTableCell();
            week.EnableViewState = false;
            week.Attributes.Add("class", "weekno");
            week.InnerHtml = GetWeekNumber(idxDate).ToString();
            row.Cells.Add(week);
            return idxDate;
        }

        private static DateTime CreateWeekDaysAtTop(DateTime idxDate, HTML.HtmlTable tbl)
        {
            HTML.HtmlTableRow rowDays = new HTML.HtmlTableRow();
            rowDays.EnableViewState = false;
            rowDays.Cells.Add(new HTML.HtmlTableCell());
            rowDays.Attributes.Add("class", "weekdays");
            DateTime idxWeekDay = idxDate;
            for (int idx = 0; idx < 7; idx++)
            {
                HTML.HtmlTableCell cell = new HTML.HtmlTableCell();
                cell.EnableViewState = false;
                cell.InnerHtml = idxWeekDay.ToString("ddd", System.Threading.Thread.CurrentThread.CurrentUICulture);
                rowDays.Cells.Add(cell);
                idxWeekDay = idxWeekDay.AddDays(1);
            }
            tbl.Rows.Add(rowDays);
            return idxDate;
        }

        private int GetWeekNumber(DateTime dt)
        {
            int year = dt.Year;
            DateTime week1;
            if (dt >= new DateTime(year, 12, 29))
            {
                week1 = GetWeekOneDate(year + 1);
                if (dt < week1)
                {
                    week1 = GetWeekOneDate(year);
                }
            }
            else
            {
                week1 = GetWeekOneDate(year);
                if (dt < week1)
                {
                    week1 = GetWeekOneDate(--year);
                }
            }
            return ((dt - week1).Days / 7 + 1);
        }

        private DateTime GetWeekOneDate(int year)
        {
            DateTime date = new DateTime(year, 1, 4);
            int dayNum = (int)date.DayOfWeek; // 0==Sunday, 6==Saturday
            if (dayNum == 0)
                dayNum = 7;
            return date.AddDays(1 - dayNum);
        }

        private void CreateYearMonthPicker(HTML.HtmlTable tbl)
        {
            // Creating header row (with month and year picker)
            HTML.HtmlTableRow headerRow = new HTML.HtmlTableRow();
            headerRow.EnableViewState = false;
            headerRow.ID = "head_" + Value.ToString("dd_MM_yyyy", System.Globalization.CultureInfo.InvariantCulture);
            HTML.HtmlTableCell headerCell = new HTML.HtmlTableCell();
            headerCell.EnableViewState = false;
            headerCell.Style["text-align"] = "center";
            headerCell.ID = "headC_" + Value.ToString("dd_MM_yyyy", System.Globalization.CultureInfo.InvariantCulture);
            headerCell.ColSpan = 8;

            // Year SelectList
            CreateYearPicker(headerCell);

            // Month SelectList
            CreateMonthPicker(headerCell);

            headerRow.Cells.Add(headerCell);
            tbl.Rows.Add(headerRow);
        }

        private void CreateMonthPicker(HTML.HtmlTableCell headerCell)
        {
            SelectList month = new SelectList();
            month.EnableViewState = false;
            month.ID = "month_" + Value.ToString("dd_MM_yyyy", System.Globalization.CultureInfo.InvariantCulture);
            for (int idxMonth = 1; idxMonth < 13; idxMonth++)
            {
                ListItem idxItem = new ListItem();
                idxItem.Text = new DateTime(Value.Year, idxMonth, 1).ToString("MMM", System.Threading.Thread.CurrentThread.CurrentUICulture);
                idxItem.Value = idxMonth.ToString();
                if (idxMonth == Value.Month)
                    month.SelectedItem = idxItem;
                month.Items.Add(idxItem);
            }
            month.SelectedIndexChanged += new EventHandler(month_SelectedIndexChanged);
            headerCell.Controls.Add(month);
        }

        private void CreateYearPicker(HTML.HtmlTableCell headerCell)
        {
            SelectList year = new SelectList();
            year.EnableViewState = false;
            year.ID = "year_" + Value.ToString("dd_MM_yyyy", System.Globalization.CultureInfo.InvariantCulture);
            for (int idxYear = Value.Year - 25; idxYear < Value.Year + 25; idxYear++)
            {
                ListItem idxItem = new ListItem();
                idxItem.Text = idxYear.ToString();
                idxItem.Value = idxYear.ToString();
                if (idxYear == Value.Year)
                    year.SelectedItem = idxItem;
                year.Items.Add(idxItem);
            }
            year.SelectedIndexChanged += new EventHandler(year_SelectedIndexChanged);
            headerCell.Controls.Add(year);
        }

        private void today_Click(object sender, EventArgs e)
        {
            Value = DateTime.Now;
            _content.ReRender();
            _content.Controls.Clear();
            CreateCalendarControls();
            if (SelectedValueChanged != null)
                SelectedValueChanged(this, new EventArgs());
            if (DateClicked != null)
                DateClicked(this, new EventArgs());
        }

        private void month_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectList drop = sender as SelectList;
            int newMonth = Int32.Parse(drop.SelectedItem.Value);
            DateTime newValue = new DateTime(Value.Year, newMonth, Math.Min(28, Value.Day), Value.Hour, Value.Minute, 0);
            if (Value.Date != newValue.Date)
            {
                Value = newValue;
                _content.ReRender();
                _content.Controls.Clear();
                CreateCalendarControls();
                if (SelectedValueChanged != null)
                    SelectedValueChanged(this, new EventArgs());
            }
        }

        private void year_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectList drop = sender as SelectList;
            int newYear = Int32.Parse(drop.SelectedItem.Value);
            DateTime newValue = new DateTime(newYear, Value.Month, Math.Min(28, Value.Day), Value.Hour, Value.Minute, 0);
            if (Value.Date != newValue.Date)
            {
                Value = newValue;
                _content.ReRender();
                _content.Controls.Clear();
                CreateCalendarControls();
                if (SelectedValueChanged != null)
                    SelectedValueChanged(this, new EventArgs());
            }
        }

        private void btn_Click(object sender, EventArgs e)
        {
            LinkButton button = sender as LinkButton;
            DateTime newValue = DateTime.ParseExact(button.ID, "yyyy_MM_dd", System.Globalization.CultureInfo.InvariantCulture);
            if (Value.Date != newValue.Date)
            {
                Value = new DateTime(newValue.Year, newValue.Month, newValue.Day, Value.Hour, Value.Minute, 0);
                _content.ReRender();
                _content.Controls.Clear();
                CreateCalendarControls();
                if (SelectedValueChanged != null)
                    SelectedValueChanged(this, new EventArgs());
            }
            if (DateClicked != null)
                DateClicked(this, new EventArgs());
        }
    }
}
