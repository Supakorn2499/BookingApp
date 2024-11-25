const Dashboard = () => ({ language = 'th' }) => {
    const data = [
      { name: "Jan", total: 1200 },
      { name: "Feb", total: 900 },
      { name: "Mar", total: 1600 },
      { name: "Apr", total: 1400 },
      { name: "May", total: 2000 },
      { name: "Jun", total: 1800 },
    ];
  
    const translations = {
      th: {
        title: 'แดชบอร์ด',
        overview: 'ภาพรวม',
        sales: 'ยอดขาย',
        metrics: 'ตัวชี้วัด'
      },
      en: {
        title: 'Dashboard',
        overview: 'Overview',
        sales: 'Sales',
        metrics: 'Metrics'
      }
    };
  
    const currentLanguage = translations[language] ? language : 'th';
    const t = translations[currentLanguage];
  
    return (
      <div className="space-y-6">
        <h2 className="text-lg font-bold">{t.title}</h2>
      </div>
    );
  };

  export default Dashboard;