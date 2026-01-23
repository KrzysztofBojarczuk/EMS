import { Card } from "primereact/card";
import { UserVehiclesStats } from "../../../Models/Vehicle";
import { formatCurrency } from "../../Utils/Currency";

interface Props {
  userVehiclesStats: UserVehiclesStats | null;
}

const VehicleStats = ({ userVehiclesStats }: Props) => {
  if (!userVehiclesStats) {
    return null;
  }

  return (
    <Card title="Statistics" className="mt-5">
      <div className="grid">
        <div className="col text-sm font-semibold">
          Active: {userVehiclesStats.activeVehicles}
        </div>
        <div className="col text-sm font-semibold">
          Inactive: {userVehiclesStats.inactiveVehicles}
        </div>
        <div className="col text-sm font-semibold">
          Avg age: {userVehiclesStats.averageVehicleAge.toFixed(1)} years
        </div>
        <div className="col text-sm font-semibold">
          Total OC: {formatCurrency(userVehiclesStats.totalInsuranceCost)}
        </div>
        <div className="col text-sm font-semibold">
          Avg OC: {formatCurrency(userVehiclesStats.averageInsuranceCost)}
        </div>
      </div>
    </Card>
  );
};
export default VehicleStats;
