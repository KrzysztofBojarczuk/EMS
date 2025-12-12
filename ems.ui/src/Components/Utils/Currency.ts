export const formatCurrency = (value: number) => {
  return value.toLocaleString("de-DE", { style: "currency", currency: "EUR" });
};
